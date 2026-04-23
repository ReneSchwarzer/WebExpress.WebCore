using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPackage;
using WebExpress.WebCore.WebPackage.Model;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Unit tests for the package manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestPackageManager
    {
        /// <summary>
        /// Tests the register function of the package manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateComponentHubMock();
            var packageManager = componentHub.PackageManager as PackageManager;

            // act
            Assert.NotNull(packageManager);
        }

        /// <summary>
        /// Tests the remove function of the package manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateComponentHubMock();
            var packageManager = componentHub.PackageManager as PackageManager;

            // act
            Assert.NotNull(packageManager);
        }

        /// <summary>
        /// Tests whether the package manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var packageManager = componentHub.PackageManager as PackageManager;

            // act
            Assert.True(typeof(IComponentManager).IsAssignableFrom(packageManager.GetType()));
        }

        /// <summary>
        /// Tests adding a package and firing the AddPackage event.
        /// </summary>
        [Fact]
        public void AddPackageEvent()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateComponentHubMock();
            var packageManager = componentHub.PackageManager as PackageManager;
            bool eventFired = false;
            packageManager.AddPackage += (sender, item) => { eventFired = true; };

            // create dummy package
            var package = new PackageCatalogItem() { Id = "test", File = "test.wxp", State = PackageCatalogeItemState.Active };

            // act
            var method = typeof(PackageManager).GetMethod("OnAddPackage", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(packageManager, [package]);

            // validation
            Assert.True(eventFired);
        }

        /// <summary>
        /// Tests removing a package and firing the RemovePackage event.
        /// </summary>
        [Fact]
        public void RemovePackageEvent()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateComponentHubMock();
            var packageManager = componentHub.PackageManager as PackageManager;
            bool eventFired = false;
            packageManager.RemovePackage += (sender, item) => { eventFired = true; };

            // create dummy package
            var package = new PackageCatalogItem() { Id = "test", File = "test.wxp", State = PackageCatalogeItemState.Active };

            // act
            var method = typeof(PackageManager).GetMethod("OnRemovePackage", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(packageManager, [package]);

            Assert.True(eventFired);
        }

        /// <summary>
        /// Tests that a package can be added, scanned and detected as new.
        /// </summary>
        [Fact]
        public void ScanDetectsNewPackage()
        {
            // arrange
            var httpServerContext = UnitTestFixture.CreateHttpServerContextMock();
            var componentHub = UnitTestFixture.CreateComponentHubMock(httpServerContext);
            var packageManager = componentHub.PackageManager as PackageManager;
            var packagePath = httpServerContext.PackagePath;
            var dummyFile = Path.Combine(packagePath, "dummy.wxp");

            try
            {
                // create dummy package zip file with valid .spec inside
                Directory.CreateDirectory(packagePath);

                using (var zip = ZipFile.Open(dummyFile, ZipArchiveMode.Create))
                {
                    var entry = zip.CreateEntry("dummy.spec");
                    using var writer = new StreamWriter(entry.Open());
                    writer.Write(@"
                    <package>
                        <id>dummy</id>
                        <version>1.0.0</version>
                        <title>DummyTitle</title>
                        <authors>UnitTest</authors>
                    </package>");
                }

                // act - scan should detect the new file
                packageManager.Scan();

                // validation
                Assert.Contains(packageManager.Catalog.Packages, x => x.File == "dummy.wxp");

            }
            finally
            {
                // cleanup
                File.Delete(dummyFile);
                Directory.Delete(packagePath, true);
            }
        }

        /// <summary>
        /// Tests that removing a package file triggers its removal from the catalog.
        /// </summary>
        [Fact]
        public void ScanDetectsRemovedPackage()
        {
            // arrange
            var httpServerContext = UnitTestFixture.CreateHttpServerContextMock();
            var componentHub = UnitTestFixture.CreateComponentHubMock(httpServerContext);
            var packageManager = componentHub.PackageManager as PackageManager;
            var packagePath = httpServerContext.PackagePath;
            var dummyFile = Path.Combine(packagePath, "dummy.wxp");

            try
            {
                // place and scan dummy package file
                Directory.CreateDirectory(packagePath);

                using (var zip = ZipFile.Open(dummyFile, ZipArchiveMode.Create))
                {
                    var entry = zip.CreateEntry("dummy.spec");
                    using var writer = new StreamWriter(entry.Open());
                    writer.Write(@"
                    <package>
                        <id>dummy</id>
                        <version>1.0.0</version>
                        <title>DummyTitle</title>
                        <authors>UnitTest</authors>
                    </package>");
                }

                packageManager.Scan();
                Assert.Contains(packageManager.Catalog.Packages, x => x.File == "dummy.wxp");

                // remove file and scan again
                File.Delete(dummyFile);

                // act - scan should detect the removed file
                packageManager.Scan();

                // validation
                Assert.DoesNotContain(packageManager.Catalog.Packages, x => x.File == "dummy.wxp");

            }
            finally
            {
                // cleanup
                File.Delete(dummyFile);
                Directory.Delete(packagePath, true);
            }
        }

        /// <summary>
        /// Tests loading package metadata from a package file.
        /// </summary>
        [Fact]
        public void LoadPackageReadsSpec()
        {
            // arrange
            var httpServerContext = UnitTestFixture.CreateHttpServerContextMock();
            var componentHub = UnitTestFixture.CreateComponentHubMock(httpServerContext);
            var packageManager = componentHub.PackageManager as PackageManager;
            var packagePath = httpServerContext.PackagePath;
            var dummyFile = Path.Combine(packagePath, "dummy.wxp");

            try
            {
                // create minimal dummy .wxp with .spec inside
                Directory.CreateDirectory(packagePath);

                using (var zip = ZipFile.Open(dummyFile, ZipArchiveMode.Create))
                {
                    var entry = zip.CreateEntry("dummy.spec");
                    using var writer = new StreamWriter(entry.Open());
                    writer.Write(@"
                    <package>
                        <id>dummy</id>
                        <version>1.0.0</version>
                        <title>DummyTitle</title>
                        <authors>UnitTest</authors>
                    </package>");
                }
                // use private LoadPackage method via reflection
                var method = typeof(PackageManager).GetMethod("LoadPackage", BindingFlags.NonPublic | BindingFlags.Instance);

                // act
                var result = method.Invoke(packageManager, [dummyFile]) as PackageCatalogItem;

                // validation
                Assert.NotNull(result);
                Assert.Equal("dummy", result?.Id);
                Assert.Equal("DummyTitle", result?.Metadata.Title);
            }
            finally
            {
                // cleanup
                File.Delete(dummyFile);
                Directory.Delete(packagePath, true);
            }
        }

        /// <summary>
        /// Tests package validation with extension/type checks.
        /// </summary>
        [Fact]
        public void ValidatePackageRejectsInvalidExtension()
        {
            // arrange
            var httpServerContext = UnitTestFixture.CreateHttpServerContextMock();
            var componentHub = UnitTestFixture.CreateComponentHubMock(httpServerContext);
            var packageManager = componentHub.PackageManager as PackageManager;
            var packagePath = httpServerContext.PackagePath;
            var dummyFile = Path.Combine(packagePath, "dummy.zip");

            try
            {
                Directory.CreateDirectory(packagePath);
                File.WriteAllText(dummyFile, "not-a-package");

                // act
                var validation = packageManager.ValidatePackage(dummyFile);

                // validation
                Assert.False(validation.IsValid);
                Assert.Contains(validation.Messages, x => x.Contains("extension", StringComparison.OrdinalIgnoreCase));
            }
            finally
            {
                if (File.Exists(dummyFile))
                {
                    File.Delete(dummyFile);
                }

                if (Directory.Exists(packagePath))
                {
                    Directory.Delete(packagePath, true);
                }
            }
        }

        /// <summary>
        /// Tests a complete package lifecycle using explicit package manager operations.
        /// </summary>
        [Fact]
        public void PackageLifecycleInstallDeactivateActivateUninstall()
        {
            // arrange
            var httpServerContext = UnitTestFixture.CreateHttpServerContextMock();
            var componentHub = UnitTestFixture.CreateComponentHubMock(httpServerContext);
            var packageManager = componentHub.PackageManager as PackageManager;
            var packagePath = httpServerContext.PackagePath;
            var packageFile = Path.Combine(packagePath, "lifecycle.1.0.0.wxp");

            try
            {
                Directory.CreateDirectory(packagePath);
                CreatePackageArchive(packageFile, "lifecycle", "1.0.0");

                // act + validation (install active)
                var install = packageManager.InstallPackage(packageFile, true);
                Assert.True(install.Success);
                Assert.Equal(PackageCatalogeItemState.Active, packageManager.GetPackage("lifecycle")?.State);

                // act + validation (deactivate)
                var deactivate = packageManager.DeactivatePackage("lifecycle");
                Assert.True(deactivate.Success);
                Assert.Equal(PackageCatalogeItemState.Disable, packageManager.GetPackage("lifecycle")?.State);

                // act + validation (activate)
                var activate = packageManager.ActivatePackage("lifecycle");
                Assert.True(activate.Success);
                Assert.Equal(PackageCatalogeItemState.Active, packageManager.GetPackage("lifecycle")?.State);

                // act + validation (uninstall)
                var uninstall = packageManager.UninstallPackage("lifecycle");
                Assert.True(uninstall.Success);
                Assert.Null(packageManager.GetPackage("lifecycle"));
            }
            finally
            {
                if (Directory.Exists(packagePath))
                {
                    Directory.Delete(packagePath, true);
                }
            }
        }

        /// <summary>
        /// Tests that update fails when uploaded package id does not match the requested package id.
        /// </summary>
        [Fact]
        public void UpdatePackageRejectsMismatchedId()
        {
            // arrange
            var httpServerContext = UnitTestFixture.CreateHttpServerContextMock();
            var componentHub = UnitTestFixture.CreateComponentHubMock(httpServerContext);
            var packageManager = componentHub.PackageManager as PackageManager;
            var packagePath = httpServerContext.PackagePath;
            var packageFile = Path.Combine(packagePath, "other.1.0.0.wxp");

            try
            {
                Directory.CreateDirectory(packagePath);
                CreatePackageArchive(packageFile, "other", "1.0.0");

                // act
                var result = packageManager.UpdatePackage("expected", packageFile);

                // validation
                Assert.False(result.Success);
                Assert.Contains("does not match", result.Message, StringComparison.OrdinalIgnoreCase);
            }
            finally
            {
                if (Directory.Exists(packagePath))
                {
                    Directory.Delete(packagePath, true);
                }
            }
        }

        /// <summary>
        /// Tests package SHA-256 verification support.
        /// </summary>
        [Fact]
        public void ValidatePackageWithSha256()
        {
            // arrange
            var httpServerContext = UnitTestFixture.CreateHttpServerContextMock();
            var componentHub = UnitTestFixture.CreateComponentHubMock(httpServerContext);
            var packageManager = componentHub.PackageManager as PackageManager;
            var packagePath = httpServerContext.PackagePath;
            var packageFile = Path.Combine(packagePath, "signed.1.0.0.wxp");

            try
            {
                Directory.CreateDirectory(packagePath);
                CreatePackageArchive(packageFile, "signed", "1.0.0");
                var expectedHash = ComputeSha256(packageFile);

                // act
                var valid = packageManager.ValidatePackage(packageFile, expectedSha256: expectedHash);
                var invalid = packageManager.ValidatePackage(packageFile, expectedSha256: "deadbeef");

                // validation
                Assert.True(valid.IsValid);
                Assert.False(invalid.IsValid);
            }
            finally
            {
                if (Directory.Exists(packagePath))
                {
                    Directory.Delete(packagePath, true);
                }
            }
        }

        /// <summary>
        /// Creates a simple package archive for tests.
        /// </summary>
        /// <param name="file">The package file path.</param>
        /// <param name="id">The package id.</param>
        /// <param name="version">The package version.</param>
        private static void CreatePackageArchive(string file, string id, string version)
        {
            using var zip = ZipFile.Open(file, ZipArchiveMode.Create);
            var specEntry = zip.CreateEntry($"{id}.spec");
            using (var writer = new StreamWriter(specEntry.Open()))
            {
                writer.Write($@"
                    <package>
                        <id>{id}</id>
                        <version>{version}</version>
                        <title>{id}-title</title>
                        <authors>UnitTest</authors>
                    </package>");
            }

            // add minimal lib folder marker to resemble package layout
            zip.CreateEntry("lib/");
        }

        /// <summary>
        /// Computes SHA-256 for a test file.
        /// </summary>
        /// <param name="file">The file path.</param>
        /// <returns>The sha-256 hash as lowercase hex.</returns>
        private static string ComputeSha256(string file)
        {
            using var stream = File.OpenRead(file);
            var hash = SHA256.HashData(stream);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }
    }
}
