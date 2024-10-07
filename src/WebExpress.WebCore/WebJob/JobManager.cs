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
    /// Processing of cyclic jobs.
    /// </summary>
    public sealed class JobManager : IComponentManagerPlugin, ISystemComponent, IExecutableElements
    {
        private readonly IComponentHub _componentManager;
        private readonly IHttpServerContext _httpServerContext;
        private readonly ScheduleDictionary _staticScheduleDictionary = [];
        private readonly List<ScheduleDynamicItem> _dynamicScheduleList = [];
        private readonly CancellationTokenSource _tokenSource = new();
        private readonly Clock _clock = new();

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentManager">The component manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        internal JobManager(IComponentHub componentManager, IHttpServerContext httpServerContext)
        {
            _componentManager = componentManager;

            _componentManager.PluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            _componentManager.PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
            };

            _componentManager.ModuleManager.AddModule += (sender, moduleContext) =>
            {
                AssignToModule(moduleContext);
            };

            _componentManager.ModuleManager.RemoveModule += (sender, moduleContext) =>
            {
                DetachFromModule(moduleContext);
            };

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate
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
                    _httpServerContext.Log.Warning
                    (
                        I18N.Translate
                        (
                            "webexpress:jobmanager.moduleless", id
                        )
                    );
                }

                // register the job
                if (!_staticScheduleDictionary.ContainsKey(pluginContext))
                {
                    _staticScheduleDictionary.Add(pluginContext, new List<ScheduleStaticItem>());
                }

                var dictItem = _staticScheduleDictionary[pluginContext];

                dictItem.Add(new ScheduleStaticItem()
                {
                    Assembly = assembly,
                    JobId = id,
                    Type = job,
                    Cron = new Cron(pluginContext.Host, minute, hour, day, month, weekday),
                    moduleId = moduleId
                });

                _httpServerContext.Log.Debug
                (
                    I18N.Translate
                    (
                        "webexpress:jobmanager.job.register", moduleId, id
                    )
                );

                // assign the job to existing modules.
                foreach (var moduleContext in _componentManager.ModuleManager.GetModules(pluginContext, moduleId))
                {
                    if (moduleContext.PluginContext != pluginContext)
                    {
                        // job is not part of the module
                        _httpServerContext.Log.Warning
                        (
                            I18N.Translate
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

            _dynamicScheduleList.Append(item);

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

            _dynamicScheduleList.Append(item);

            return jobInstance;
        }

        /// <summary>
        /// Assign existing job to the module.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        private void AssignToModule(IModuleContext moduleContext)
        {
            foreach (var scheduleItem in _staticScheduleDictionary.Values.SelectMany(x => x))
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
            foreach (var scheduleItem in _staticScheduleDictionary.Values.SelectMany(x => x))
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
            if (pluginContext == null || !_staticScheduleDictionary.ContainsKey(pluginContext))
            {
                return Enumerable.Empty<ScheduleStaticItem>();
            }

            return _staticScheduleDictionary[pluginContext];
        }

        /// <summary>
        /// Executes the schedule.
        /// </summary>
        internal void Execute()
        {
            Task.Factory.StartNew(() =>
            {
                while (!_tokenSource.IsCancellationRequested)
                {
                    Update();

                    var secendsLeft = 60 - DateTime.Now.Second;
                    Thread.Sleep(secendsLeft * 1000);
                }

            }, _tokenSource.Token);
        }

        /// <summary>
        /// Run jobs on demand (concurrent execution).
        /// </summary>
        private void Update()
        {
            foreach (var clock in _clock.Synchronize())
            {
                foreach (var scheduleItemValue in _staticScheduleDictionary.Values
                    .SelectMany(x => x)
                    .SelectMany(x => x.Dictionary.Values))
                {
                    if (scheduleItemValue.JobContext.Cron.Matching(_clock))
                    {
                        _httpServerContext.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress:jobmanager.job.process",
                                scheduleItemValue.JobContext.JobId
                            )
                        );

                        Task.Factory.StartNew(() =>
                        {
                            scheduleItemValue.Instance?.Process();
                        }, _tokenSource.Token);
                    }
                }

                foreach (var scheduleItemValue in _dynamicScheduleList)
                {
                    if (scheduleItemValue.JobContext.Cron.Matching(_clock))
                    {
                        _httpServerContext.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress:jobmanager.job.process",
                                scheduleItemValue.JobContext.JobId
                            )
                        );

                        Task.Factory.StartNew(() =>
                        {
                            scheduleItemValue.Instance?.Process();
                        }, _tokenSource.Token);
                    }
                }
            }
        }

        /// <summary>
        /// Stop running the scheduler.
        /// </summary>
        public void ShutDown()
        {
            _tokenSource.Cancel();
        }

        /// <summary>
        /// Removes all jobs associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the jobs to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            if (pluginContext == null)
            {
                return;
            }

            // the plugin has not been registered in the manager
            if (!_staticScheduleDictionary.ContainsKey(pluginContext))
            {
                return;
            }

            foreach (var scheduleItem in _staticScheduleDictionary[pluginContext])
            {
                scheduleItem.Dispose();
            }

            _staticScheduleDictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Removes a job.
        /// </summary>
        /// <param name="job">The job to remove.</param>
        public void Remove(IJob job)
        {
            _dynamicScheduleList.RemoveAll(x => x == job);
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
                    I18N.Translate
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
