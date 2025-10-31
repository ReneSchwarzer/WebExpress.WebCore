using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebPackage.Model;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebPackage
{
    /// <summary>
    /// The package manager manages packages with WebExpress extensions. The packages 
    /// must be in WebExpressPackage format (*.wxp).
    /// </summary>
    public sealed class PackageManager : IPackageManager, ISystemComponent
    {
        private readonly ComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;
        private readonly PluginManager _pluginManager;

        /// <summary>
        /// An event that fires when an package is added.
        /// </summary>
        public event EventHandler<PackageCatalogItem> AddPackage;

        /// <summary>
        /// An event that fires when an package is removed.
        /// </summary>
        public event EventHandler<PackageCatalogItem> RemovePackage;

        /// <summary>
        /// Thread Termination.
        /// </summary>
        private CancellationTokenSource TokenSource { get; } = new CancellationTokenSource();

        /// <summary>
        /// Returns the catalog of installed packages.
        /// </summary>
        public PackageCatalog Catalog { get; } = new PackageCatalog();

        /// <summary>
        /// Synchronization object for scanning and mutating catalog.
        /// </summary>
        private readonly Lock _scanLock = new();

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="pluginManager">The plugin manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used via Reflection.")]
        private PackageManager(IComponentHub componentHub, IPluginManager pluginManager, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub as ComponentHub;
            _pluginManager = pluginManager as PluginManager;

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress.webcore:packagemanager.initialization")
            );
        }

        /// <summary>
        /// Starts the manager.
        /// </summary>
        internal void Execute()
        {
            // load the default plugins
            _pluginManager.Register();

            // boot default elements 
            _componentHub.BootComponent(_pluginManager.Plugins);

            LoadCatalog();

            foreach (var package in Catalog.Packages)
            {
                var packagesFromFile = LoadPackage(Path.Combine(_httpServerContext.PackagePath, package.File));

                package.Metadata = packagesFromFile?.Metadata;

                _httpServerContext.Log.Debug
                (
                    I18N.Translate("webexpress.webcore:packagemanager.existing", package.File)
                );

                if (package.State != PackageCatalogeItemState.Disable)
                {
                    package.State = PackageCatalogeItemState.Active;
                    ExtractPackage(package);
                    RegisterPackage(package);
                    BootPackage(package);
                }
            }

            SaveCatalog();

            // build sitemap
            _componentHub.SitemapManager.Refresh();

            Task.Factory.StartNew(() =>
            {
                while (!TokenSource.IsCancellationRequested)
                {
                    Scan();

                    var secendsLeft = 60 - DateTime.Now.Second;
                    Thread.Sleep(secendsLeft * 1000);
                }

            }, TokenSource.Token);
        }

        /// <summary>
        /// Stop running the manager.
        /// </summary>
        public void ShutDown()
        {
            TokenSource.Cancel();
        }

        /// <summary>
        /// Searches the package directory for new, changed or removed packages.
        /// </summary>
        public void Scan()
        {
            lock (_scanLock)
            {
                _httpServerContext.Log.Debug
                (
                    I18N.Translate
                    (
                        "webexpress.webcore:packagemanager.scan",
                        _httpServerContext.PackagePath
                    )
                );

                // determine all WebExpress packages from the file system
                var packageFiles = Directory.GetFiles(_httpServerContext.PackagePath, "*.wxp").Select(x => Path.GetFileName(x)).ToList();

                // all packages that are not yet installed
                var newPackages = packageFiles.Except(Catalog.Packages.Where(x => x != null).Select(x => x.File)).ToList();

                // all packages that are no longer available
                var removePackages = Catalog.Packages.Where(x => x != null).Select(x => x.File).Except(packageFiles).ToList();

                // determine changed packages by comparing spec version and relevant metadata
                var changedPackages = new List<string>();
                foreach (var existing in Catalog.Packages.Where(x => x != null))
                {
                    var fullPath = Path.Combine(_httpServerContext.PackagePath, existing.File);
                    if (!File.Exists(fullPath))
                    {
                        continue;
                    }

                    var fromFile = LoadPackage(fullPath);
                    if (fromFile == null)
                    {
                        continue;
                    }

                    if (HasPackageChanged(existing, fromFile))
                    {
                        changedPackages.Add(existing.File);
                    }
                }

                foreach (var package in newPackages)
                {
                    var packagesFromFile = LoadPackage(Path.Combine(_httpServerContext.PackagePath, package));
                    if (packagesFromFile == null)
                    {
                        continue;
                    }

                    packagesFromFile.State = PackageCatalogeItemState.Active;

                    ExtractPackage(packagesFromFile);
                    RegisterPackage(packagesFromFile);
                    BootPackage(packagesFromFile);

                    Catalog.Packages.Add(packagesFromFile);

                    // raise event for added package
                    OnAddPackage(packagesFromFile);

                    _httpServerContext.Log.Debug
                    (
                        I18N.Translate
                        (
                            "webexpress.webcore:packagemanager.add",
                            package
                        )
                    );
                }

                foreach (var package in changedPackages)
                {
                    var existing = Catalog.Packages.FirstOrDefault(x => x != null && x.File == package);
                    if (existing == null)
                    {
                        continue;
                    }

                    var fromFile = LoadPackage(Path.Combine(_httpServerContext.PackagePath, package));
                    if (fromFile == null)
                    {
                        continue;
                    }

                    // respect disabled state; only update metadata without activating
                    if (existing.State == PackageCatalogeItemState.Disable)
                    {
                        existing.Metadata = fromFile.Metadata;
                        _httpServerContext.Log.Debug($"package '{package}' metadata updated while disabled");
                    }
                    else
                    {
                        // deactivate and unload old plugin instances
                        DeactivateAndUnregisterPackage(existing);
                        // cleanup extracted content
                        RemoveExtractedDirectory(existing);

                        // update metadata and identification
                        existing.Id = fromFile.Id;
                        existing.Metadata = fromFile.Metadata;
                        existing.State = PackageCatalogeItemState.Active;

                        // extract, register and boot new content
                        ExtractPackage(existing);
                        RegisterPackage(existing);
                        BootPackage(existing);

                        _httpServerContext.Log.Debug($"package '{package}' updated and reloaded");
                    }
                }

                foreach (var package in removePackages)
                {
                    var existing = Catalog.Packages.FirstOrDefault(x => x != null && x.File == package);
                    if (existing == null)
                    {
                        continue;
                    }

                    // deactivate and unload all plugins related to the package
                    DeactivateAndUnregisterPackage(existing);

                    // cleanup extracted directory
                    RemoveExtractedDirectory(existing);

                    // raise event before removing from catalog
                    OnRemovePackage(existing);

                    // remove package from catalog
                    Catalog.Packages.Remove(existing);

                    _httpServerContext.Log.Debug
                    (
                        I18N.Translate
                        (
                            "webexpress.webcore:packagemanager.remove",
                            package
                        )
                    );
                }

                if (newPackages.Count != 0 || removePackages.Count != 0 || changedPackages.Count != 0)
                {
                    // build sitemap
                    _componentHub.SitemapManager.Refresh();

                    // save the catalog
                    SaveCatalog();
                }
            }
        }

        /// <summary>
        /// Opens a package and finds the meta information.
        /// </summary>
        /// <param name="file">The path and file name.</param>
        /// <returns>The package information as a catalog entry.</returns>
        private PackageCatalogItem LoadPackage(string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    using var zip = ZipFile.Open(file, ZipArchiveMode.Read);

                    var specEntry = zip.Entries.Where(x => Path.GetExtension(x.FullName) == ".spec").FirstOrDefault();
                    if (specEntry == null)
                    {
                        _httpServerContext.Log.Warning($"package spec was not found in '{file}'");
                        return null;
                    }

                    var serializer = new XmlSerializer(typeof(PackageItemSpec));
                    PackageItemSpec spec;
                    using (var stream = specEntry.Open())
                    {
                        spec = (PackageItemSpec)serializer.Deserialize(stream);
                    }

                    return new PackageCatalogItem()
                    {
                        Id = spec.Id,
                        File = Path.GetFileName(file),
                        State = PackageCatalogeItemState.Available,
                        Metadata = new PackageItem()
                        {
                            FileName = Path.GetFileName(file),
                            Id = spec.Id,
                            Version = spec.Version,
                            Title = spec.Title,
                            Authors = spec.Authors,
                            License = spec.License,
                            Icon = spec.Icon,
                            Readme = spec.Readme,
                            Description = spec.Description,
                            Tags = spec.Tags,
                            PluginSources = spec.Plugins
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                _httpServerContext.Log.Exception(ex);
            }

            _httpServerContext.Log.Debug
            (
                I18N.Translate
                (
                    "webexpress.webcore:packagemanager.packagenotfound",
                    file
                )
            );

            return null;
        }

        /// <summary>
        /// Load the catalog.
        /// </summary>
        private void LoadCatalog()
        {
            var catalogeFile = Path.Combine(_httpServerContext.PackagePath, "catalog.xml");
            if (File.Exists(catalogeFile))
            {
                using var catalog = new StreamReader(catalogeFile);

                if (catalog.BaseStream.Length == 0)
                {
                    return;
                }

                var serializer = new XmlSerializer(typeof(PackageCatalog));
                var items = (PackageCatalog)serializer.Deserialize(catalog);

                Catalog.Packages.Clear();
                //Catalog.Packages.RemoveAll(x => !x.System);
                Catalog.Packages.AddRange(items.Packages);
            }

            Log();
        }

        /// <summary>
        /// Save the catalog.
        /// </summary>
        private void SaveCatalog()
        {
            var catalogeFile = Path.Combine(_httpServerContext.PackagePath, "catalog.xml");

            using var fs = new FileStream(catalogeFile, FileMode.Create);
            using var writer = new XmlTextWriter(fs, Encoding.Unicode);
            var serializer = new XmlSerializer(typeof(PackageCatalog));

            writer.Formatting = Formatting.Indented;
            serializer.Serialize(writer, Catalog, new XmlSerializerNamespaces([new XmlQualifiedName("", "")]));

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress.webcore:packagemanager.save")
            );
        }

        /// <summary>
        /// Extracts the specified package to the file system.
        /// </summary>
        /// <param name="package">The package.</param>
        private void ExtractPackage(PackageCatalogItem package)
        {
            var packageFile = Path.Combine(_httpServerContext.PackagePath, package?.File);

            if (File.Exists(packageFile))
            {
                using var zip = ZipFile.Open(packageFile, ZipArchiveMode.Read);

                var extractedPath = Path.Combine(_httpServerContext.PackagePath, Path.GetFileNameWithoutExtension(package?.File));

                if (!Directory.Exists(extractedPath))
                {
                    Directory.CreateDirectory(extractedPath);
                }

                foreach (var entry in zip.Entries.Where(x => Path.GetDirectoryName(x.FullName).StartsWith("lib", StringComparison.OrdinalIgnoreCase)))
                {
                    // directory entries in the zip have an empty Name
                    if (string.IsNullOrEmpty(entry.Name))
                    {
                        var dirPath = Path.Combine(extractedPath, entry.FullName);
                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }

                        continue;
                    }

                    var targetFilePath = Path.Combine(extractedPath, entry.FullName);
                    var targetDir = Path.GetDirectoryName(targetFilePath);

                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    if (!File.Exists(targetFilePath))
                    {
                        entry.ExtractToFile(targetFilePath);
                    }
                }
            }
        }

        /// <summary>
        /// Registers the plungins included in the package.
        /// </summary>
        /// <param name="package">The package.</param>
        private void RegisterPackage(PackageCatalogItem package)
        {
            // load plugins
            foreach (var plugin in package?.Metadata?.PluginSources ?? [])
            {
                var pluginContexts = _pluginManager.Register(GetTargetPath(package, plugin));

                package.Plugins.AddRange(pluginContexts);
            }
        }

        /// <summary>
        /// Boots the components included in the package.
        /// </summary>
        /// <param name="package">The package.</param>
        private void BootPackage(PackageCatalogItem package)
        {
            _componentHub.BootComponent(package.Plugins);
        }

        /// <summary>
        /// Determines the target directory where the plug-ins of the package are located 
        /// for the current target platform.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="plugin">The plugin.</param>
        /// <returns>The directory (absolutely).</returns>
        private string GetTargetPath(PackageCatalogItem package, string plugin)
        {
            return Path.GetFullPath(Path.Combine
            (
                _httpServerContext.PackagePath,
                Path.GetFileNameWithoutExtension(package?.File), plugin, GetTFM(), $"{Path.GetFileName(plugin)}.dll"
            ));
        }

        /// <summary>
        /// Determines the target framework.
        /// </summary>
        /// <returns>The TFM</returns>
        private static string GetTFM()
        {
            var targetFrameworkAttribute = Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(TargetFrameworkAttribute), false)
                    .Select(x => x as TargetFrameworkAttribute)
                    .SingleOrDefault();

            return targetFrameworkAttribute.FrameworkDisplayName.Replace(" ", "").ToLower().Replace(".net", "net");
        }

        /// <summary>
        /// Raises the AddPackage event.
        /// </summary>
        /// <param name="item">The package catalog item.</param>
        private void OnAddPackage(PackageCatalogItem item)
        {
            AddPackage?.Invoke(this, item);
        }

        /// <summary>
        /// Raises the RemovePackage event.
        /// </summary>
        /// <param name="item">The package catalog item.</param>
        private void OnRemovePackage(PackageCatalogItem item)
        {
            RemovePackage?.Invoke(this, item);
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        private void Log()
        {
            if (Catalog.Packages.Count == 0)
            {
                return;
            }

            using var frame = new LogFrameSimple(_httpServerContext.Log);
            var list = new List<string>
            {
                I18N.Translate("webexpress.webcore:packagemanager.titel")
            };

            foreach (var package in Catalog.Packages)
            {
                list.Add
                (
                    I18N.Translate("webexpress.webcore:packagemanager.package", package.Id)
                );
            }

            _httpServerContext.Log.Info(string.Join(Environment.NewLine, list));
        }

        /// <summary>
        /// Checks if a package has changed by comparing spec-relevant metadata.
        /// </summary>
        /// <param name="existing">The existing catalog item.</param>
        /// <param name="fromFile">The catalog item loaded from file.</param>
        /// <returns>True if changed; otherwise false.</returns>
        private static bool HasPackageChanged(PackageCatalogItem existing, PackageCatalogItem fromFile)
        {
            if (existing == null || fromFile == null)
            {
                return false;
            }

            // if no metadata was present, treat as no change and let metadata be assigned on next run
            if (existing.Metadata == null || fromFile.Metadata == null)
            {
                return false;
            }

            if (!string.Equals(existing.Metadata.Version, fromFile.Metadata.Version, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // compare plugin sources sequence-insensitively
            var a = (existing.Metadata.PluginSources ?? []).OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToArray();
            var b = (fromFile.Metadata.PluginSources ?? []).OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToArray();

            if (a.Length != b.Length)
            {
                return true;
            }

            for (int i = 0; i < a.Length; i++)
            {
                if (!string.Equals(a[i], b[i], StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gracefully deactivates a package, shutting down and removing its plugins and 
        /// clearing the plugin list.
        /// </summary>
        /// <param name="package">The package.</param>
        private void DeactivateAndUnregisterPackage(PackageCatalogItem package)
        {
            if (package == null)
            {
                return;
            }

            // shut down components associated to each plugin and remove the plugin
            foreach (var pluginContext in package.Plugins.ToList())
            {
                _componentHub.ShutDownComponent(pluginContext);
                _pluginManager.Remove(pluginContext);
            }

            package.Plugins.Clear();
            package.State = PackageCatalogeItemState.Available;
        }

        /// <summary>
        /// Removes the extracted directory for a package if it exists.
        /// </summary>
        /// <param name="package">The package.</param>
        private void RemoveExtractedDirectory(PackageCatalogItem package)
        {
            var extractedPath = Path.Combine(_httpServerContext.PackagePath, Path.GetFileNameWithoutExtension(package?.File));
            try
            {
                if (Directory.Exists(extractedPath))
                {
                    Directory.Delete(extractedPath, true);
                }
            }
            catch (Exception ex)
            {
                // keep running even if cleanup fails
                _httpServerContext.Log.Exception(ex);
            }
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}