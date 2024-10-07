using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Serialization;
using WebExpress.WebCore.Config;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebPackage;
using WebExpress.WebCore.WebUri;

[assembly: InternalsVisibleTo("WebExpress.WebCore.Test")]

namespace WebExpress.WebCore
{
    /// <summary>
    /// The class provides a web server application for WebExpress.
    /// </summary>
    public class WebEx
    {
        private static ComponentHub _componentHub;
        private HttpServer _httpServer;

        /// <summary>
        /// Returns or sets the name of the web server.
        /// </summary>
        public string Name { get; set; } = "WebExpress";

        /// <summary>
        /// Returns the program version.
        /// </summary>
        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Returns the component hub.
        /// </summary>
        public static IComponentHub ComponentHub => _componentHub;

        /// <summary>
        /// Entry point of application.
        /// </summary>
        /// <param name="args">Call arguments.</param>
        /// <returns>The return code. 0 on success. A number greater than 0 for errors.</returns>
        public static int Main(string[] args)
        {
            var app = new WebEx()
            {
                Name = Assembly.GetExecutingAssembly().GetName().Name
            };

            return app.Execution(args);
        }

        /// <summary>
        /// Running the application.
        /// </summary>
        /// <param name="args">Call arguments.</param>
        /// <returns>The return code. 0 on success. A number greater than 0 for errors.</returns>
        public int Execution(string[] args)
        {
            // prepare call arguments
            ArgumentParser.Current.Register(new ArgumentParserCommand() { FullName = "help", ShortName = "h" });
            ArgumentParser.Current.Register(new ArgumentParserCommand() { FullName = "config", ShortName = "c" });
            ArgumentParser.Current.Register(new ArgumentParserCommand() { FullName = "port", ShortName = "p" });
            ArgumentParser.Current.Register(new ArgumentParserCommand() { FullName = "spec", ShortName = "s" });
            ArgumentParser.Current.Register(new ArgumentParserCommand() { FullName = "output", ShortName = "o" });
            ArgumentParser.Current.Register(new ArgumentParserCommand() { FullName = "target", ShortName = "t" });

            // parsing call arguments
            var argumentDict = ArgumentParser.Current.Parse(args);

            if (argumentDict.ContainsKey("help"))
            {
                Console.WriteLine(Name + " [-port number | -config dateiname | -help]");
                Console.WriteLine("Version: " + Version);

                return 0;
            }

            // package builder
            if (argumentDict.ContainsKey("spec") || argumentDict.ContainsKey("output"))
            {
                if (!argumentDict.ContainsKey("spec"))
                {
                    Console.WriteLine("*** PackageBuilder: The spec file (-s) was not specified.");

                    return 1;
                }

                if (!argumentDict.ContainsKey("config"))
                {
                    Console.WriteLine("*** PackageBuilder: The config (-c) was not specified.");

                    return 1;
                }

                if (!argumentDict.ContainsKey("target"))
                {
                    Console.WriteLine("*** PackageBuilder: The target framework (-t) was not specified.");

                    return 1;
                }

                if (!argumentDict.ContainsKey("output"))
                {
                    Console.WriteLine("*** PackageBuilder: The output directory (-o) was not specified.");

                    return 1;
                }

                PackageBuilder.Create(argumentDict["spec"], argumentDict["config"], argumentDict["target"], argumentDict["output"]);

                return 0;
            }

            // configuration
            if (!argumentDict.ContainsKey("config"))
            {
                // check if there is a file called config.xml
                if (!File.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, "config"), "webexpress.config.xml")))
                {
                    Console.WriteLine("No configuration file was specified. Usage: " + Name + " -config filename");

                    return 1;
                }

                argumentDict.Add("config", "webexpress.config.xml");
            }

            // initialization of the web server
            Initialization(ArgumentParser.Current.GetValidArguments(args), Path.Combine(Path.Combine(Environment.CurrentDirectory, "config"), argumentDict["config"]));

            // start the manager
            _componentHub.Execute();

            // starting the web server
            Start();

            // finish
            Exit();

            return 0;
        }

        /// <summary>
        /// Called when the application is to be terminated using Ctrl+C.
        /// </summary>
        /// <param name="sender">The trigger of the event.</param>
        /// <param name="e">The event argument.</param>
        private void OnCancel(object sender, ConsoleCancelEventArgs e)
        {
            Exit();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="args">The valid arguments.</param>
        /// <param param name="configFile">The configuration file.</param>
        private void Initialization(string args, string configFile)
        {
            // Config laden
            using var reader = new FileStream(configFile, FileMode.Open);
            var serializer = new XmlSerializer(typeof(HttpServerConfig));
            var config = serializer.Deserialize(reader) as HttpServerConfig;
            var log = new Log();

            var culture = CultureInfo.CurrentCulture;

            try
            {
                culture = new CultureInfo(config.Culture);

                CultureInfo.CurrentCulture = culture;
            }
            catch
            {

            }

            var packageBase = string.IsNullOrWhiteSpace(config.PackageBase) ?
                Environment.CurrentDirectory : Path.IsPathRooted(config.PackageBase) ?
                config.PackageBase :
                Path.Combine(Environment.CurrentDirectory, config.PackageBase);

            var assetBase = string.IsNullOrWhiteSpace(config.AssetBase) ?
                Environment.CurrentDirectory : Path.IsPathRooted(config.AssetBase) ?
                config.AssetBase :
                Path.Combine(Environment.CurrentDirectory, config.AssetBase);

            var dataBase = string.IsNullOrWhiteSpace(config.DataBase) ?
                Environment.CurrentDirectory : Path.IsPathRooted(config.DataBase) ?
                config.DataBase :
                Path.Combine(Environment.CurrentDirectory, config.DataBase);

            var context = new HttpServerContext
            (
                config.Uri,
                config.Endpoints,
                Path.GetFullPath(packageBase),
                Path.GetFullPath(assetBase),
                Path.GetFullPath(dataBase),
                Path.GetDirectoryName(configFile),
                new UriResource(config.ContextPath),
                culture,
                log,
                null
            );

            _httpServer = new HttpServer(context)
            {
                Config = config
            };

            _componentHub = CreateComponentManager(_httpServer.HttpServerContext);

            // start logging
            _httpServer.HttpServerContext.Log.Begin(config.Log);

            // log program start
            _httpServer.HttpServerContext.Log.Seperator('/');
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.startup"));
            _httpServer.HttpServerContext.Log.Info(message: "".PadRight(80, '-'));
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.version"), args: Version);
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.arguments"), args: args);
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.workingdirectory"), args: Environment.CurrentDirectory);
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.packagebase"), args: config.PackageBase);
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.assetbase"), args: config.AssetBase);
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.database"), args: config.DataBase);
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.configurationdirectory"), args: Path.GetDirectoryName(configFile));
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.configuration"), args: Path.GetFileName(configFile));
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.logdirectory"), args: Path.GetDirectoryName(_httpServer.HttpServerContext.Log.Filename));
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.log"), args: Path.GetFileName(_httpServer.HttpServerContext.Log.Filename));
            foreach (var v in config.Endpoints)
            {
                _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.uri"), args: v.Uri);
            }

            _httpServer.HttpServerContext.Log.Seperator('=');

            if (!Directory.Exists(config.PackageBase))
            {
                Directory.CreateDirectory(config.PackageBase);
            }

            if (!Directory.Exists(config.AssetBase))
            {
                Directory.CreateDirectory(config.AssetBase);
            }

            if (!Directory.Exists(config.DataBase))
            {
                Directory.CreateDirectory(config.DataBase);
            }

            Console.CancelKeyPress += OnCancel;
        }

        /// <summary>
        /// Start the web server.
        /// </summary>
        private void Start()
        {
            _httpServer.Start();

            Thread.CurrentThread.Join();
        }

        /// <summary>
        /// Quits the application.
        /// </summary>
        private void Exit()
        {
            _httpServer.Stop();

            // end of program log
            _httpServer.HttpServerContext.Log.Seperator('=');
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.errors"), args: _httpServer.HttpServerContext.Log.ErrorCount);
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.warnings"), args: _httpServer.HttpServerContext.Log.WarningCount);
            _httpServer.HttpServerContext.Log.Info(message: I18N.Translate("webexpress:app.done"));
            _httpServer.HttpServerContext.Log.Seperator('/');

            // Stop running
            _componentHub.ShutDown();

            // stop logging
            _httpServer.HttpServerContext.Log.Close();
        }

        /// <summary>
        /// Returns a component based on its id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The instance of the component or null.</returns>
        public static IComponentManager GetComponent(string id)
        {
            return _componentHub.GetComponent(id);
        }

        /// <summary>
        /// Returns a component based on its type.
        /// </summary>
        /// <typeparam name="T">The component class.</typeparam>
        /// <returns>The instance of the component or null.</returns>
        public static T GetComponent<T>() where T : IComponentManager
        {
            return _componentHub.GetComponent<T>();
        }

        /// <summary>
        /// Creates and returns a new instance of <see cref="ComponentHub"/>.
        /// </summary>
        /// <param name="httpServerContext">The HTTP server context used to initialize the component manager.</param>
        /// <returns>A new instance of <see cref="ComponentHub"/>.</returns>
        protected virtual ComponentHub CreateComponentManager(IHttpServerContext httpServerContext)
        {
            return new ComponentHub(httpServerContext);
        }
    }
}
