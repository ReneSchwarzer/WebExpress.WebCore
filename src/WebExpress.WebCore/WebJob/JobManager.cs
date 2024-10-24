using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebJob.Model;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebJob
{
    /// <remarks>
    /// This class manages the processing of cyclic jobs. It provides methods to register, remove, and execute jobs.
    /// </remarks>
    public sealed class JobManager : IJobManager, ISystemComponent, IExecutableElements
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
        /// Returns all job contextes.
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
        private JobManager(IComponentHub componentManager, IHttpServerContext httpServerContext)
        {
            _componentHub = componentManager;

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication += OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication += OnRemoveApplication;
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
        /// Registers a dynamic job.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="cron">The cropn-object.</param>
        /// <returns>The job.</returns>
        private IJob Register<T>(IPluginContext pluginContext, Cron cron) where T : IJob
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
        /// Discovers and binds static jobs to an application.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin whose jobs are to be associated.</param>
        private void Register(IPluginContext pluginContext)
        {
            if (_staticScheduleDictionary.ContainsKey(pluginContext))
            {
                return;
            }

            Register(pluginContext, _componentHub.ApplicationManager.GetApplications(pluginContext));
        }

        /// <summary>
        /// Discovers and binds jobs to an application.
        /// </summary>
        /// <param name="applicationContext">The context of the application whose jobs are to be associated.</param>
        private void Register(IApplicationContext applicationContext)
        {
            foreach (var pluginContext in _componentHub.PluginManager.GetPlugins(applicationContext))
            {
                if (_staticScheduleDictionary.TryGetValue(pluginContext, out var appDict) && appDict.ContainsKey(applicationContext))
                {
                    continue;
                }

                Register(pluginContext, [applicationContext]);
            }
        }

        /// <summary>
        /// Registers resources for a given plugin and application context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context (optional).</param>
        private void Register(IPluginContext pluginContext, IEnumerable<IApplicationContext> applicationContexts)
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

                foreach (var customAttribute in job.CustomAttributes.Where(x => x.AttributeType == typeof(JobAttribute)))
                {
                    minute = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    hour = customAttribute.ConstructorArguments.Skip(1).FirstOrDefault().Value?.ToString();
                    day = customAttribute.ConstructorArguments.Skip(2).FirstOrDefault().Value?.ToString();
                    month = customAttribute.ConstructorArguments.Skip(3).FirstOrDefault().Value?.ToString();
                    weekday = customAttribute.ConstructorArguments.Skip(4).FirstOrDefault().Value?.ToString();
                }

                // assign the job to existing applications
                foreach (var applicationContext in _componentHub.ApplicationManager.GetApplications(pluginContext))
                {
                    var jobContext = new JobContext()
                    {
                        JobId = job.FullName.ToLower(),
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        Cron = new Cron(_httpServerContext, minute, hour, day, month, weekday),
                    };

                    if (job != default)
                    {
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
        }

        /// <summary>
        /// Removes all jobs associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the jobs to remove.</param>
        internal void Remove(IPluginContext pluginContext)
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
                OnRemoveJob(scheduleItem.JobContext);
                scheduleItem.Dispose();
            }

            _staticScheduleDictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Removes all jobs associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the jobs to remove.</param>
        internal void Remove(IApplicationContext applicationContext)
        {
            if (applicationContext == null)
            {
                return;
            }

            foreach (var pluginDict in _staticScheduleDictionary.Values)
            {
                foreach (var appDict in pluginDict.Where(x => x.Key == applicationContext).Select(x => x.Value))
                {
                    foreach (var scheduleItem in appDict.Values.SelectMany(x => x))
                    {
                        OnRemoveJob(scheduleItem.JobContext);
                        scheduleItem.Dispose();
                    }
                }

                pluginDict.Remove(applicationContext);
            }
        }

        /// <summary>
        /// Removes a dynamic job.
        /// </summary>
        /// <param name="job">The job to remove.</param>
        public void Remove(IJob job)
        {
            _dynamicScheduleList.RemoveAll(x => x == job);
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
        /// Handles the event when an plugin is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the plugin being added.</param>
        private void OnAddPlugin(object sender, IPluginContext e)
        {
            Register(e);
        }

        /// <summary>  
        /// Handles the event when a plugin is removed.  
        /// </summary>  
        /// <param name="sender">The source of the event.</param>  
        /// <param name="e">The context of the plugin being removed.</param>  
        private void OnRemovePlugin(object sender, IPluginContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Handles the event when an application is removed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being removed.</param>
        private void OnRemoveApplication(object sender, IApplicationContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Handles the event when an application is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being added.</param>
        private void OnAddApplication(object sender, IApplicationContext e)
        {
            Register(e);
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
        /// Returns a JobContext instance associated with an application.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="jobType">The type of the job.</param>
        /// <returns>A JobContext instance.</returns>
        public IJobContext GetJob(IApplicationContext applicationContext, Type jobType)
        {
            return _staticScheduleDictionary
                .SelectMany(x => x.Value)
                .SelectMany(x => x.Value)
                .SelectMany(x => x.Value)
                .Union(_dynamicScheduleList.Select(x => x))
                .FirstOrDefault(x => x.JobContext.ApplicationContext == applicationContext && x.JobClass == jobType)?.JobContext;
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

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
            _componentHub.PluginManager.AddPlugin -= OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin -= OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication -= OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication -= OnRemoveApplication;

            _tokenSource.Cancel();
        }
    }
}
