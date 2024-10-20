using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebJob.Model;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebJob
{
    /// <remarks>
    /// This class manages the processing of cyclic jobs. It provides methods to register, remove, and execute jobs.
    /// </remarks>
    public sealed class JobManager : IJobManager, IComponentManagerPlugin, ISystemComponent, IExecutableElements
    {
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;
        private readonly ScheduleDictionary _staticScheduleDictionary = [];
        private readonly List<ScheduleItem> _dynamicScheduleList = [];
        private readonly CancellationTokenSource _tokenSource = new();
        private readonly Clock _clock = new();

        /// <summary>
        /// An event that fires when an job is added.
        /// </summary>
        public event EventHandler<IJobContext> AddJob;

        /// <summary>
        /// An event that fires when an job is removed.
        /// </summary>
        public event EventHandler<IJobContext> RemoveJob;

        /// <summary>
        /// Returns all jobs contextes.
        /// </summary>
        public IEnumerable<IJobContext> Jobs => _staticScheduleDictionary
            .SelectMany(x => x.Value)
            .SelectMany(x => x.Value)
            .SelectMany(x => x.Value)
            .Select(x => x.JobContext)
            .Union(_dynamicScheduleList.Select(x => x.JobContext));

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentManager">The component manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        internal JobManager(IComponentHub componentManager, IHttpServerContext httpServerContext)
        {
            _componentHub = componentManager;

            _componentHub.PluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            _componentHub.PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
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
                var applicationIds = new List<string>();
                var minute = "*";
                var hour = "*";
                var day = "*";
                var month = "*";
                var weekday = "*";

                foreach (var customAttribute in job.CustomAttributes.Where(x => x.AttributeType == typeof(JobAttribute)))
                {
                    minute = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    hour = customAttribute.ConstructorArguments.Skip(1).FirstOrDefault().Value?.ToString();
                    day = customAttribute.ConstructorArguments.Skip(2).FirstOrDefault().Value?.ToString();
                    month = customAttribute.ConstructorArguments.Skip(3).FirstOrDefault().Value?.ToString();
                    weekday = customAttribute.ConstructorArguments.Skip(4).FirstOrDefault().Value?.ToString();
                }

                foreach (var customAttribute in job.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IEventAttribute))))
                {
                    if (customAttribute.AttributeType.Name == typeof(ApplicationAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ApplicationAttribute<>).Namespace)
                    {
                        applicationIds.Add(customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower());
                    }
                }

                if (!applicationIds.Any())
                {
                    // no application specified
                    _httpServerContext.Log.Warning
                    (
                        I18N.Translate("webexpress:jobmanager.applicationless", id)
                    );

                    break;
                }

                if (applicationIds.Count() > 1)
                {
                    // too many specified applications
                    _httpServerContext.Log.Warning
                    (
                        I18N.Translate("webexpress:jobmanager.applicationrich", id)
                    );
                }

                // assign the module to existing applications.
                var applicationContext = _componentHub.ApplicationManager.GetApplication(applicationIds.FirstOrDefault());

                if (job != default)
                {
                    var jobContext = new JobContext()
                    {
                        JobId = job.FullName.ToLower(),
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        Cron = new Cron(pluginContext.Host, minute, hour, day, month, weekday),
                    };

                    if (_staticScheduleDictionary.AddScheduleItem
                    (
                        pluginContext,
                        applicationContext,
                        new ScheduleItem(_componentHub, pluginContext, applicationContext, jobContext, job)
                    ))
                    {
                        OnAddJob(jobContext);

                        _httpServerContext.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress:jobmanager.register",
                                id,
                                applicationContext.ApplicationId
                            )
                        );
                    }
                    else
                    {
                        _httpServerContext.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress:jobmanager.duplicate",
                                id,
                                applicationContext.ApplicationId
                            )
                        );
                    }
                }
                else
                {
                    _httpServerContext.Log.Debug
                    (
                        I18N.Translate
                        (
                            "webexpress:jobmanager.jobless",
                            id
                        )
                    );
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
            var jobContext = new JobContext()
            {
                PluginContext = pluginContext,
                JobId = typeof(T).FullName?.ToLower(),
                Cron = cron
            };

            var item = new ScheduleItem(_componentHub, pluginContext, null, jobContext, typeof(T));

            _dynamicScheduleList.Append(item);

            OnAddJob(jobContext);

            return item.Instance;
        }

        /// <summary>
        /// Raises the AddJob event.
        /// </summary>
        /// <param name="jobContext">The job context.</param>
        private void OnAddJob(IJobContext jobContext)
        {
            AddJob?.Invoke(this, jobContext);
        }

        /// <summary>
        /// Raises the RemoveJob event.
        /// </summary>
        /// <param name="jobContext">The job context.</param>
        private void OnRemoveJob(IJobContext jobContext)
        {
            RemoveJob?.Invoke(this, jobContext);
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
                foreach (var scheduleItemValue in _staticScheduleDictionary
                    .SelectMany(x => x.Value)
                    .SelectMany(x => x.Value)
                    .SelectMany(x => x.Value)
                    .Union(_dynamicScheduleList.Select(x => x)))
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

            foreach (var scheduleItem in _staticScheduleDictionary[pluginContext].Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x))
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
            foreach (var scheduleItem in Jobs.Where(x => x.PluginContext == pluginContext))
            {
                output.Add
                (
                    string.Empty.PadRight(deep) +
                    I18N.Translate
                    (
                        "webexpress:jobmanager.job",
                        scheduleItem.JobId,
                        scheduleItem.ApplicationContext
                    )
                );
            }
        }
    }
}
