using System;
using System.Collections.Generic;
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
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebPackage
{
    /// <summary>
    /// The package manager manages packages with WebExpress extensions. The packages must be in WebExpressPackage format (*.wxp).
    /// </summary>
    public sealed class PackageManager : IComponent, ISystemComponent
    {
        /// <summary>
        /// An event that fires when an package is added.
        /// </summary>
        public static event EventHandler<PackageCatalogItem> AddPackage;

        /// <summary>
        /// An event that fires when an package is removed.
        /// </summary>
        public static event EventHandler<PackageCatalogItem> RemovePackage;

        /// <summary>
        /// Returns or sets the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Thread Termination.
        /// </summary>
        private CancellationTokenSource TokenSource { get; } = new CancellationTokenSource();

        /// <summary>
        /// Returns the catalog of installed packages.
        /// </summary>
        private PackageCatalog Catalog { get; } = new PackageCatalog();

        /// <summary>
        /// Constructor
        /// </summary>
        internal PackageManager()
        {

        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The reference to the context of the host.</param>
        public void Initialization(IHttpServerContext context)
        {
            HttpServerContext = context;

            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N("webexpress:packagemanager.initialization")
            );
        }

        /// <summary>
        /// Starts the manager.
        /// </summary>
        internal void Execute()
        {
            // load the default plugins
            ComponentManager.PluginManager.Register();

            // boot default elements 
            ComponentManager.BootComponent(ComponentManager.PluginManager.Plugins);

            LoadCatalog();

            foreach (var package in Catalog.Packages)
            {
                var packagesFromFile = LoadPackage(Path.Combine(HttpServerContext.PackagePath, package.File));

                package.Metadata = packagesFromFile?.Metadata;

                HttpServerContext.Log.Debug
                (
                    InternationalizationManager.I18N("webexpress:packagemanager.existing", package.File)
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
            ComponentManager.SitemapManager.Refresh();

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
            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N
                (
                    "webexpress:packagemanager.scan",
                    HttpServerContext.PackagePath
                )
            );

            // determine all WebExpress packages from the file system
            var packageFiles = Directory.GetFiles(HttpServerContext.PackagePath, "*.wxp").Select(x => Path.GetFileName(x)).ToList();

            // all packages that are not yet installed
            var newPackages = packageFiles.Except(Catalog.Packages.Where(x => x != null).Select(x => x.File)).ToList();

            // all packages that are already installed
            //var existingPackages = Catalog.Packages.Select(x => x.File);

            // all packages that are no longer available
            var removePackages = Catalog.Packages.Where(x => x != null).Select(x => x.File).Except(packageFiles).ToList();

            foreach (var package in newPackages)
            {
                var packagesFromFile = LoadPackage(Path.Combine(HttpServerContext.PackagePath, package));

                ExtractPackage(packagesFromFile);
                RegisterPackage(packagesFromFile);
                BootPackage(packagesFromFile);

                Catalog.Packages.Add(packagesFromFile);

                HttpServerContext.Log.Debug
                (
                    InternationalizationManager.I18N
                    (
                        "webexpress:packagemanager.add",
                        package
                    )
                );
            }

            foreach (var package in removePackages)
            {
                var packagesFromFile = LoadPackage(Path.Combine(HttpServerContext.PackagePath, package));

                Catalog.Packages.Add(packagesFromFile);

                HttpServerContext.Log.Debug
                (
                    InternationalizationManager.I18N
                    (
                        "webexpress:packagemanager.remove",
                        package
                    )
                );
            }

            // 2. alle WebExpress-Pakete aus dem Filesystem ermitteln 


            // 2. Installiere Pakete ermitteln
            var packagesFromCatalog = Catalog.Packages;



            // 2 . DLL extrahieren
            //webExpressPackages.ForEach(x => x.Files);


            //foreach ()
            //{
            //    if (!Packages.ContainsKey(packagefile))
            //    {
            //        var package = Package.Open(packagefile);
            //        Console.WriteLine("Nuspec File: " + package.FileName);
            //        Console.WriteLine("Nuspec Id: " + package.Id);
            //        Console.WriteLine("Nuspec Version: " + package.Version);
            //        Console.WriteLine("Nuspec Autoren: " + package.Authors);
            //        Console.WriteLine("Nuspec License: " + package.License);
            //        Console.WriteLine("Nuspec LicenseUrl: " + package.LicenseUrl);
            //        Console.WriteLine("Nuspec Description: " + package.Description);
            //        Console.WriteLine("Nuspec Repository: " + package.Repository);
            //        Console.WriteLine("Nuspec Abhängigkeiten: " + string.Join(",", package.Dependencies.Select(x => x.Id)));

            //        Packages.Add(packagefile, package);
            //    }
            //}

            if (newPackages.Any() || removePackages.Any())
            {
                // build sitemap
                ComponentManager.SitemapManager.Refresh();

                // save the catalog
                SaveCatalog();
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
                    var serializer = new XmlSerializer(typeof(PackageItemSpec));
                    var spec = (PackageItemSpec)serializer.Deserialize(specEntry.Open());
                    //        var files = new List<Tuple<string, byte[]>>();

                    //        foreach (ZipArchiveEntry entry in zip.Entries.Where(x => Path.GetDirectoryName(x.FullName).StartsWith("lib")))
                    //        {
                    //            Console.WriteLine("Lib: " + entry?.FullName);

                    //            using var stream = entry?.Open();
                    //            using MemoryStream ms = new MemoryStream();
                    //            stream.CopyTo(ms);
                    //            files.Add(new Tuple<string, byte[]>(entry?.FullName, ms.ToArray()));
                    //        }

                    //        foreach (ZipArchiveEntry entry in zip.Entries.Where(x => Path.GetDirectoryName(x.FullName).StartsWith("runtimes")))
                    //        {
                    //            Console.WriteLine("Runtimes: " + entry?.FullName);

                    //            using var stream = entry?.Open();
                    //            using MemoryStream ms = new MemoryStream();
                    //            stream.CopyTo(ms);
                    //            files.Add(new Tuple<string, byte[]>(entry?.FullName, ms.ToArray()));
                    //        }

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
                HttpServerContext.Log.Exception(ex);
            }

            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N
                (
                    "webexpress:packagemanager.packagenotfound",
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
            var catalogeFile = Path.Combine(HttpServerContext.PackagePath, "catalog.xml");
            if (File.Exists(catalogeFile))
            {
                using var catalog = new StreamReader(catalogeFile);
                var serializer = new XmlSerializer(typeof(PackageCatalog));
                var items = (PackageCatalog)serializer.Deserialize(catalog);

                Catalog.Packages.Clear();
                //Catalog.Packages.RemoveAll(x => !x.System);
                Catalog.Packages.AddRange(items.Packages);
            }
        }

        /// <summary>
        /// Save the catalog.
        /// </summary>
        private void SaveCatalog()
        {
            var catalogeFile = Path.Combine(HttpServerContext.PackagePath, "catalog.xml");

            using var fs = new FileStream(catalogeFile, FileMode.Create);
            using var writer = new XmlTextWriter(fs, Encoding.Unicode);
            var serializer = new XmlSerializer(typeof(PackageCatalog));

            writer.Formatting = Formatting.Indented;
            serializer.Serialize(writer, Catalog, new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") }));

            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N("webexpress:packagemanager.save")
            );
        }

        /// <summary>
        /// Extracts the specified package to the file system.
        /// </summary>
        /// <param name="package">The package.</param>
        private void ExtractPackage(PackageCatalogItem package)
        {
            var packageFile = Path.Combine(HttpServerContext.PackagePath, package?.File);

            if (File.Exists(packageFile))
            {
                using var zip = ZipFile.Open(packageFile, ZipArchiveMode.Read);

                var specEntry = zip.Entries.Where(x => Path.GetExtension(x.FullName) == ".spec").FirstOrDefault();
                var extractedPath = Path.Combine(HttpServerContext.PackagePath, Path.GetFileNameWithoutExtension(package?.File));

                if (!Directory.Exists(extractedPath))
                {
                    Directory.CreateDirectory(extractedPath);
                    // deleting an existing directory
                    //Directory.Delete(extractedPath, true);
                }

                foreach (var entry in zip.Entries.Where(x => Path.GetDirectoryName(x.FullName).StartsWith("lib")))
                {
                    var entryFileName = Path.Combine(extractedPath, entry?.FullName);

                    if (entryFileName.EndsWith("/"))
                    {
                        if (!Directory.Exists(entryFileName))
                        {
                            Directory.CreateDirectory(entryFileName);
                        }
                    }
                    else
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(entryFileName)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(entryFileName));
                        }

                        if (!File.Exists(entryFileName))
                        {
                            entry.ExtractToFile(entryFileName);
                        }
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
            foreach (var plugin in package?.Metadata.PluginSources ?? Enumerable.Empty<string>())
            {
                var pluginContexts = ComponentManager.PluginManager.Register(GetTargetPath(package, plugin));

                package.Plugins.AddRange(pluginContexts);
            }

            ComponentManager.LogStatus();
        }

        /// <summary>
        /// Boots the components included in the package.
        /// </summary>
        /// <param name="package">The package.</param>
        private void BootPackage(PackageCatalogItem package)
        {
            ComponentManager.BootComponent(package.Plugins);
        }

        /// <summary>
        /// Determines the target directory where the plug-ins of the package are located for the current target platform
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="plugin">The plugin.</param>
        /// <returns>The directory (absolutely).</returns>
        private string GetTargetPath(PackageCatalogItem package, string plugin)
        {
            return Path.GetFullPath(Path.Combine
            (
                HttpServerContext.PackagePath,
                Path.GetFileNameWithoutExtension(package?.File), plugin, GetTFM(), $"{Path.GetFileName(plugin)}.dll"
            ));
        }

        /// <summary>
        /// Determines the target framework.
        /// </summary>
        /// <returns>The TFM</returns>
        private string GetTFM()
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
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
        }
    }
}
