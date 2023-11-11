using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Serialization;

namespace WebExpress.Core.WebPackage
{
    /// <summary>
    /// Class for generating webexpress packages from a build environment.
    /// </summary>
    public static class PackageBuilder
    {
        /// <summary>
        /// Creates a webex package.
        /// </summary>
        /// <param name="specFile">The spec file (*.spec).</param>
        /// <param name="config">The config. Debug or Release.</param>
        /// <param name="targets">The target frameworks. Semicolon separated list of target framework moniker (TFM).</param>
        /// <param name="outputDirectory">The output directory.</param>
        public static void Create(string specFile, string config, string targets, string outputDirectory)
        {
            Console.WriteLine($"*** PackageBuilder: specFile '{specFile}'.");
            Console.WriteLine($"*** PackageBuilder: config '{config}'.");
            Console.WriteLine($"*** PackageBuilder: targets '{targets}'.");
            Console.WriteLine($"*** PackageBuilder: outputDirectory '{outputDirectory}'.");

            var rootDirectory = Path.GetDirectoryName(specFile);
            using var fileStream = File.OpenRead(specFile);
            var serializer = new XmlSerializer(typeof(PackageItemSpec));
            var package = (PackageItemSpec)serializer.Deserialize(fileStream);
            var zipFileType = package.Id.Equals("WebExpress") ? "zip" : "wxp";

            Console.WriteLine($"*** PackageBuilder: Creates a webex package '{package.Id}' in directory '{outputDirectory}'.");

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using var zipFileStream = new FileStream(Path.Combine(outputDirectory, $"{package.Id}.{package.Version}.{zipFileType}"), FileMode.Create);
            using var archive = new ZipArchive(zipFileStream, ZipArchiveMode.Create, true);

            // find readme
            if (!string.IsNullOrWhiteSpace(package.Readme))
            {
                var file = Find(rootDirectory, Path.GetFileName(package.Readme));

                if (File.Exists(file))
                {
                    ReadmeToZip
                    (
                        archive,
                        File.ReadAllBytes(file)
                    );
                }
            }

            // privacypolicy.md
            if (!string.IsNullOrWhiteSpace(package.PrivacyPolicy))
            {
                var file = Find(rootDirectory, Path.GetFileName(package.PrivacyPolicy));

                if (File.Exists(file))
                {
                    PrivacyPolicyToZip
                    (
                        archive,
                        File.ReadAllBytes(file)
                    );
                }
            }

            // find icon
            if (!string.IsNullOrWhiteSpace(package.Icon))
            {
                var file = Find(rootDirectory, Path.GetFileName(package.Icon));

                if (File.Exists(file))
                {
                    IconToZip
                    (
                        archive,
                        Path.GetFileName(package.Icon),
                        File.ReadAllBytes(file)
                    );
                }
            }

            // find licenses 
            foreach (var file in Directory.GetFiles(Path.GetDirectoryName(specFile), "*.lic", SearchOption.AllDirectories))
            {
                if (File.Exists(file))
                {
                    LicensesToZip
                    (
                        archive,
                        Path.GetFileName(file),
                        File.ReadAllBytes(file)
                    );
                }
            }

            if (!package.Id.Equals("WebExpress"))
            {
                SpecToZip(archive, package);
            }

            ProjectToZip(archive, package, rootDirectory, config, targets);
            ArtifactsToZip(archive, package, rootDirectory);
        }

        /// <summary>
        /// Create the readme file.
        /// </summary>
        /// <param name="archive">The zip archive.</param>
        /// <param name="readme">The readme content.</param>
        private static void ReadmeToZip(ZipArchive archive, byte[] readme)
        {
            var zipArchiveEntry = archive.CreateEntry("readme.md", CompressionLevel.Fastest);
            using var zipStream = zipArchiveEntry.Open();
            zipStream.Write(readme, 0, readme.Length);

            Console.WriteLine($"*** PackageBuilder: Create the readme file.");
        }

        /// <summary>
        /// Create the privacy policy file.
        /// </summary>
        /// <param name="archive">The zip archive.</param>
        /// <param name="readme">The privacy policy content.</param>
        private static void PrivacyPolicyToZip(ZipArchive archive, byte[] readme)
        {
            var zipArchiveEntry = archive.CreateEntry("privacypolicy.md", CompressionLevel.Fastest);
            using var zipStream = zipArchiveEntry.Open();
            zipStream.Write(readme, 0, readme.Length);

            Console.WriteLine($"*** PackageBuilder: Create the privacy policy file.");
        }

        /// <summary>
        /// Create the icon file.
        /// </summary>
        /// <param name="archive">The zip archive.</param>
        /// <param name="fileName">The icon file name.</param>
        /// <param name="icon">The icon content.</param>
        private static void IconToZip(ZipArchive archive, string fileName, byte[] icon)
        {
            var zipArchiveEntry = archive.CreateEntry($"icon{Path.GetExtension(fileName)}", CompressionLevel.Fastest);
            using var zipStream = zipArchiveEntry.Open();
            zipStream.Write(icon, 0, icon.Length);

            Console.WriteLine($"*** PackageBuilder: Create the icon file.");
        }

        /// <summary>
        /// Create the licenses file.
        /// </summary>
        /// <param name="archive">The zip archive.</param>
        /// <param name="fileName">The licenses file name.</param>
        /// <param name="icon">The licenses content.</param>
        private static void LicensesToZip(ZipArchive archive, string fileName, byte[] icon)
        {
            var zipArchiveEntry = archive.CreateEntry($"licenses/{Path.GetFileNameWithoutExtension(fileName)}.txt", CompressionLevel.Fastest);
            using var zipStream = zipArchiveEntry.Open();
            zipStream.Write(icon, 0, icon.Length);

            Console.WriteLine($"*** PackageBuilder: Create the licenses file.");
        }

        /// <summary>
        /// Create the spec file.
        /// </summary>
        /// <param name="archive">The zip archive.</param>
        /// <param name="package">The package.</param>
        private static void SpecToZip(ZipArchive archive, PackageItemSpec package)
        {
            var zipBinarys = package.Id.Equals("WebExpress") ? "bin" : "lib";
            var zipArchiveEntry = archive.CreateEntry($"{package.Id}.spec", CompressionLevel.Fastest);
            var serializer = new XmlSerializer(typeof(PackageItemSpec));
            using var zipStream = zipArchiveEntry.Open();

            var newPackage = new PackageItemSpec()
            {
                Id = package.Id,
                Version = package.Version,
                Title = package.Title,
                Authors = package.Authors,
                License = package.License,
                LicenseUrl = package.LicenseUrl,
                Icon = $"icon{Path.GetExtension(package.Icon)}",
                Readme = $"readme.md",
                Description = package.Description,
                Tags = package.Tags,
                Plugins = package.Plugins?.Select(x => $"{zipBinarys}/{Path.GetFileName(x)}").ToArray(),
            };

            serializer.Serialize(zipStream, newPackage);

            Console.WriteLine($"*** PackageBuilder: Create the spec file.");
        }

        /// <summary>
        /// Copy the plugin lib files to zip.
        /// </summary>
        /// <param name="archive">The zip archive.</param>
        /// <param name="package">The package.</param>
        /// <param name="path">The root path.</param>
        /// <param name="config">The config. Debug or Release.</param>
        /// <param name="targets">The target frameworks. Semicolon separated list of target framework moniker (TFM).</param>
        private static void ProjectToZip(ZipArchive archive, PackageItemSpec package, string path, string config, string targets)
        {
            var zipBinarys = package.Id.Equals("WebExpress") ? "bin" : "lib";

            foreach (var plugin in package?.Plugins ?? Enumerable.Empty<string>())
            {
                var pluginName = Path.GetFileName(plugin);
                foreach (var target in targets?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? Enumerable.Empty<string>())
                {
                    var dir = Path.Combine(path, plugin, "bin", config, target);

                    foreach (var fileName in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
                    {
                        if (!string.IsNullOrWhiteSpace(fileName) && File.Exists(fileName))
                        {
                            var item = fileName.Replace(dir, "");
                            var fileData = File.ReadAllBytes(fileName);
                            var zipArchiveEntry = archive.CreateEntry($"{zipBinarys}/{pluginName}/{target}{item}", CompressionLevel.Fastest);
                            using var zipStream = zipArchiveEntry.Open();
                            zipStream.Write(fileData, 0, fileData.Length);

                            Console.WriteLine($"*** PackageBuilder: Copy the output file '{item}' to {pluginName}.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create the artifact files.
        /// </summary>
        /// <param name="archive">The zip archive.</param>
        /// <param name="package">The package.</param>
        /// <param name="path">The root path.</param>
        private static void ArtifactsToZip(ZipArchive archive, PackageItemSpec package, string path)
        {
            var zipBinarys = package.Id.Equals("WebExpress") ? "bin" : "lib";

            foreach (var item in package?.Artifacts ?? Enumerable.Empty<string>())
            {
                var fileName = Find(path, item);

                if (!string.IsNullOrWhiteSpace(fileName) && File.Exists(fileName))
                {
                    var fileData = File.ReadAllBytes(fileName);
                    var zipArchiveEntry = archive.CreateEntry($"{zipBinarys}/{item}", CompressionLevel.Fastest);
                    using var zipStream = zipArchiveEntry.Open();
                    zipStream.Write(fileData, 0, fileData.Length);

                    Console.WriteLine($"*** PackageBuilder: Create the artifact file '{fileName}'.");
                }
            }
        }

        /// <summary>
        /// Find a file
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>The file name, if found or null.</returns>
        private static string Find(string path, string fileName)
        {
            try
            {
                foreach (var f in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                        .Where(x => x.Replace('\\', '/').EndsWith(fileName.Replace('\\', '/'), StringComparison.OrdinalIgnoreCase)))
                {
                    return f;
                }

                path = Directory.GetParent(path)?.FullName;
            }
            catch
            {
            }

            return null;
        }
    }
}
