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
using WebExpress.WebCore.WebUri;

[assembly: InternalsVisibleTo("WebExpress.WebCore.Test")]

namespace WebExpress.WebCore
{
    public class WebEx
    {
        /// <summary>
        /// Returns or sets the name of the web server.
        /// </summary>
        public string Name { get; set; } = "WebExpress";

        /// <summary>
        /// The http(s) server.
        /// </summary>
        private HttpServer HttpServer { get; set; }

        /// <summary>
        /// Returns the program version.
        /// </summary>
        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

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

                WebPackage.PackageBuilder.Create(argumentDict["spec"], argumentDict["config"], argumentDict["target"], argumentDict["output"]);

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
            ComponentManager.Execute();

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

            HttpServer = new HttpServer(context)
            {
                Config = config
            };

            // start logging
            HttpServer.HttpServerContext.Log.Begin(config.Log);

            // log program start
            HttpServer.HttpServerContext.Log.Seperator('/');
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.startup"));
            HttpServer.HttpServerContext.Log.Info(message: "".PadRight(80, '-'));
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.version"), args: Version);
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.arguments"), args: args);
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.workingdirectory"), args: Environment.CurrentDirectory);
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.packagebase"), args: config.PackageBase);
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.assetbase"), args: config.AssetBase);
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.database"), args: config.DataBase);
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.configurationdirectory"), args: Path.GetDirectoryName(configFile));
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.configuration"), args: Path.GetFileName(configFile));
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.logdirectory"), args: Path.GetDirectoryName(HttpServer.HttpServerContext.Log.Filename));
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.log"), args: Path.GetFileName(HttpServer.HttpServerContext.Log.Filename));
            foreach (var v in config.Endpoints)
            {
                HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.uri"), args: v.Uri);
            }

            HttpServer.HttpServerContext.Log.Seperator('=');

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
            HttpServer.Start();

            Thread.CurrentThread.Join();
        }

        /// <summary>
        /// Quits the application.
        /// </summary>
        private void Exit()
        {
            HttpServer.Stop();

            // end of program log
            HttpServer.HttpServerContext.Log.Seperator('=');
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.errors"), args: HttpServer.HttpServerContext.Log.ErrorCount);
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.warnings"), args: HttpServer.HttpServerContext.Log.WarningCount);
            HttpServer.HttpServerContext.Log.Info(message: InternationalizationManager.I18N("webexpress:app.done"));
            HttpServer.HttpServerContext.Log.Seperator('/');

            // stop logging
            HttpServer.HttpServerContext.Log.Close();
        }
    }
}
