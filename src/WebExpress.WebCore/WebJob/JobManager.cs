using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebJob
{
    /// <summary>
    /// Processing of cyclic jobs
    /// </summary>
    public sealed class JobManager : IComponentPlugin, ISystemComponent, IExecutableElements
    {
        /// <summary>
        /// Thread termination.
        /// </summary>
        private CancellationTokenSource TokenSource { get; } = new CancellationTokenSource();

        /// <summary>
        /// The clock for determining the execution of the crons.
        /// </summary>
        private Clock Clock { get; } = new Clock();

        /// <summary>
        /// Returns or sets the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Returns the directory where the static jobs are listed.
        /// </summary>
        private ScheduleDictionary StaticScheduleDictionary { get; } = new ScheduleDictionary();

        /// <summary>
        /// Returns the directory where the dynamic jobs are listed.
        /// </summary>
        private IEnumerable<ScheduleDynamicItem> DynamicScheduleList { get; set; } = new List<ScheduleDynamicItem>();

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        internal JobManager()
        {
            ComponentManager.PluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            ComponentManager.PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
            };

            ComponentManager.ModuleManager.AddModule += (sender, moduleContext) =>
            {
                AssignToModule(moduleContext);
            };

            ComponentManager.ModuleManager.RemoveModule += (sender, moduleContext) =>
            {
                DetachFromModule(moduleContext);
            };
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
                InternationalizationManager.I18N
                (
                    "webexpress:jobmanager.initialization"
                )
            );
        }

        /// <summary>
        /// Discovers and registers jobs from the specified plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose jobs are to be registered.</param>
        public void Register(IPluginContext pluginContext)
        {
            var assembly = pluginContext?.Assembly;

            foreach (var job in assembly.GetTypes().Where
                (
                    x => x.IsClass == true &&
                    x.IsSealed &&
                    x.IsPublic &&
                    x.GetInterface(typeof(IJob).Name) != null
                ))
            {
                var id = job.FullName?.ToLower();
                var minute = "*";
                var hour = "*";
                var day = "*";
                var month = "*";
                var weekday = "*";
                var moduleId = string.Empty;

                foreach (var customAttribute in job.CustomAttributes.Where(x => x.AttributeType == typeof(JobAttribute)))
                {
                    minute = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    hour = customAttribute.ConstructorArguments.Skip(1).FirstOrDefault().Value?.ToString();
                    day = customAttribute.ConstructorArguments.Skip(2).FirstOrDefault().Value?.ToString();
                    month = customAttribute.ConstructorArguments.Skip(3).FirstOrDefault().Value?.ToString();
                    weekday = customAttribute.ConstructorArguments.Skip(4).FirstOrDefault().Value?.ToString();
                }

                foreach (var customAttribute in job.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IModuleAttribute))))
                {
                    if (customAttribute.AttributeType.Name == typeof(ModuleAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ModuleAttribute<>).Namespace)
                    {
                        moduleId = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower();
                    }
                }

                if (string.IsNullOrWhiteSpace(moduleId))
                {
                    // no module specified
                    HttpServerContext.Log.Warning
                    (
                        InternationalizationManager.I18N
                        (
                            "webexpress:jobmanager.moduleless", id
                        )
                    );
                }

                // register the job
                if (!StaticScheduleDictionary.ContainsKey(pluginContext))
                {
                    StaticScheduleDictionary.Add(pluginContext, new List<ScheduleStaticItem>());
                }

                var dictItem = StaticScheduleDictionary[pluginContext];

                dictItem.Add(new ScheduleStaticItem()
                {
                    Assembly = assembly,
                    JobId = id,
                    Type = job,
                    Cron = new Cron(pluginContext.Host, minute, hour, day, month, weekday),
                    moduleId = moduleId
                });

                HttpServerContext.Log.Debug
                (
                    InternationalizationManager.I18N
                    (
                        "webexpress:jobmanager.job.register", moduleId, id
                    )
                );

                // assign the job to existing modules.
                foreach (var moduleContext in ComponentManager.ModuleManager.GetModules(pluginContext, moduleId))
                {
                    if (moduleContext.PluginContext != pluginContext)
                    {
                        // job is not part of the module
                        HttpServerContext.Log.Warning
                        (
                            InternationalizationManager.I18N
                            (
                                "webexpress:jobmanager.wrongmodule",
                                moduleContext.ModuleId, id
                            )
                        );
                    }

                    AssignToModule(moduleContext);
                }
            }
        }

        /// <summary>
        /// Discovers and registers entries from the specified plugin.
        /// </summary>
        /// <param name="pluginContexts">A list with plugin contexts that contain the jobs.</param>
        public void Register(IEnumerable<IPluginContext> pluginContexts)
        {
            foreach (var pluginContext in pluginContexts)
            {
                Register(pluginContext);
            }
        }

        /// <summary>
        /// Registers a job.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="cron">The cropn-object.</param>
        /// <returns>The job.</returns>
        public IJob Register<T>(IPluginContext pluginContext, Cron cron) where T : IJob
        {
            // create context
            var jobContext = new JobContext(pluginContext)
            {
                JobId = typeof(T).FullName?.ToLower(),
                Cron = cron
            };

            var jobInstance = Activator.CreateInstance(typeof(T)) as IJob;
            jobInstance.Initialization(jobContext);

            var item = new ScheduleDynamicItem()
            {
                JobContext = jobContext,
                Instance = jobInstance
            };

            DynamicScheduleList = DynamicScheduleList.Append(item);

            return jobInstance;
        }

        /// <summary>
        /// Registers a job.
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        /// <param name="cron">The cropn-object.</param>
        /// <returns>The job.</returns>
        public IJob Register<T>(IModuleContext moduleContext, Cron cron) where T : IJob
        {
            // create context
            var jobContext = new JobContext(moduleContext)
            {
                JobId = typeof(T).FullName?.ToLower(),
                Cron = cron
            };

            var jobInstance = Activator.CreateInstance(typeof(T)) as IJob;
            jobInstance.Initialization(jobContext);

            var item = new ScheduleDynamicItem()
            {
                JobContext = jobContext,
                Instance = jobInstance
            };

            DynamicScheduleList = DynamicScheduleList.Append(item);

            return jobInstance;
        }

        /// <summary>
        /// Assign existing job to the module.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        private void AssignToModule(IModuleContext moduleContext)
        {
            foreach (var scheduleItem in StaticScheduleDictionary.Values.SelectMany(x => x))
            {
                if (scheduleItem.moduleId.Equals(moduleContext?.ModuleId))
                {
                    scheduleItem.AddModule(moduleContext);
                }
            }
        }

        /// <summary>
        /// Remove an existing modules to the job.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        private void DetachFromModule(IModuleContext moduleContext)
        {
            foreach (var scheduleItem in StaticScheduleDictionary.Values.SelectMany(x => x))
            {
                if (scheduleItem.moduleId.Equals(moduleContext?.ModuleId))
                {
                    scheduleItem.DetachModule(moduleContext);
                }
            }
        }

        /// <summary>
        /// Retruns the schedule item for a given plugin.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <returns>An enumeration of the schedule item for the given plugin.</returns>
        internal IEnumerable<ScheduleStaticItem> GetScheduleItems(IPluginContext pluginContext)
        {
            if (pluginContext == null || !StaticScheduleDictionary.ContainsKey(pluginContext))
            {
                return Enumerable.Empty<ScheduleStaticItem>();
            }

            return StaticScheduleDictionary[pluginContext];
        }

        /// <summary>
        /// Executes the schedule.
        /// </summary>
        internal void Execute()
        {
            Task.Factory.StartNew(() =>
            {
                while (!TokenSource.IsCancellationRequested)
                {
                    Update();

                    var secendsLeft = 60 - DateTime.Now.Second;
                    Thread.Sleep(secendsLeft * 1000);
                }

            }, TokenSource.Token);
        }

        /// <summary>
        /// Run jobs on demand (concurrent execution).
        /// </summary>
        private void Update()
        {
            foreach (var clock in Clock.Synchronize())
            {
                foreach (var scheduleItemValue in StaticScheduleDictionary.Values
                    .SelectMany(x => x)
                    .SelectMany(x => x.Dictionary.Values))
                {
                    if (scheduleItemValue.JobContext.Cron.Matching(Clock))
                    {
                        HttpServerContext.Log.Debug
                        (
                            InternationalizationManager.I18N
                            (
                                "webexpress:jobmanager.job.process",
                                scheduleItemValue.JobContext.JobId
                            )
                        );

                        Task.Factory.StartNew(() =>
                        {
                            scheduleItemValue.Instance?.Process();
                        }, TokenSource.Token);
                    }
                }

                foreach (var scheduleItemValue in DynamicScheduleList)
                {
                    if (scheduleItemValue.JobContext.Cron.Matching(Clock))
                    {
                        HttpServerContext.Log.Debug
                        (
                            InternationalizationManager.I18N
                            (
                                "webexpress:jobmanager.job.process",
                                scheduleItemValue.JobContext.JobId
                            )
                        );

                        Task.Factory.StartNew(() =>
                        {
                            scheduleItemValue.Instance?.Process();
                        }, TokenSource.Token);
                    }
                }
            }
        }

        /// <summary>
        /// Stop running the scheduler.
        /// </summary>
        public void ShutDown()
        {
            TokenSource.Cancel();
        }

        /// <summary>
        /// Removes all jobs associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the jobs to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            // the plugin has not been registered in the manager
            if (!StaticScheduleDictionary.ContainsKey(pluginContext))
            {
                return;
            }

            foreach (var scheduleItem in StaticScheduleDictionary[pluginContext])
            {
                scheduleItem.Dispose();
            }

            StaticScheduleDictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Removes a job.
        /// </summary>
        /// <param name="job">The job to remove.</param>
        public void Remove(IJob job)
        {
            DynamicScheduleList = DynamicScheduleList.Where(x => x != job);
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
            foreach (var scheduleItem in GetScheduleItems(pluginContext))
            {
                output.Add
                (
                    string.Empty.PadRight(deep) +
                    InternationalizationManager.I18N
                    (
                        "webexpress:jobmanager.job",
                        scheduleItem.JobId,
                        scheduleItem.ModuleContext
                    )
                );
            }
        }
    }
}
