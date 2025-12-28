using System;
using System.Linq;
using System.Reflection;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebStatusPage;

namespace WebExpress.WebCore.WebComponent
{
    /// <summary>
    /// Provides methods to create instances of components with dependency injection.
    /// </summary>
    public static class ComponentActivator
    {
        /// <summary>
        /// Creates an instance of the specified response type with the component hub and 
        /// advanced parameters.
        /// </summary>
        /// <typeparam name="TResponse">
        /// The type of the response.
        /// </typeparam>
        /// <param name="responseType">
        /// The type of the response to create.
        /// </param>
        /// <param name="httpServerContext">
        /// The reference to the context of the host.
        /// </param>
        /// <param name="componentHub">
        /// The component hub to use for dependency injection.
        /// </param>
        /// <param name="statusMessage">
        /// Additional parameter with a status message to pass to the response's constructor.
        /// </param>
        /// <param name="advancedParameters">
        /// Additional parameters to pass to the component's constructor.
        /// </param>
        /// <returns>
        /// An instance of the specified response type.
        /// </returns>
        public static TResponse CreateInstance<TResponse>(Type responseType, IHttpServerContext httpServerContext, IComponentHub componentHub, StatusMessage statusMessage, params object[] advancedParameters)
            where TResponse : Response
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = responseType?.GetConstructors(flags);

            if (constructors is not null)
            {
                foreach (var constructor in constructors.OrderByDescending(x => x.GetParameters().Length))
                {
                    // injection
                    var parameters = constructor.GetParameters();
                    var properties = componentHub.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    var parameterValues = parameters.Select(parameter =>
                        parameter.ParameterType == typeof(StatusMessage) ? statusMessage :
                        parameter.ParameterType == typeof(IComponentHub) ? componentHub :
                        parameter.ParameterType == typeof(IHttpServerContext) ? httpServerContext :
                        properties.Where(x => x.PropertyType == parameter.ParameterType)
                                  .FirstOrDefault()?
                                  .GetValue(componentHub) ??
                        advancedParameters.Where(x => x.GetType() == parameter.ParameterType)
                                  .FirstOrDefault() ?? null
                    ).ToArray();

                    if (constructor.Invoke(parameterValues) is TResponse component)
                    {
                        return component;
                    }
                }
            }

            return Activator.CreateInstance(responseType) as TResponse;
        }

        /// <summary>
        /// Creates an instance of the specified component type with the provided context, 
        /// component hub advanced parameters.
        /// </summary>
        /// <typeparam name="TComponentHub">
        /// The type of the component manager, which must implement <see cref="IComponentManager"/>.
        /// </typeparam>
        /// <param name="httpServerContext">
        /// The reference to the context of the host.
        /// </param>
        /// <param name="advancedParameters">
        /// Additional parameters to pass to the component's constructor.
        /// </param>
        /// <returns>
        /// An instance of the specified component type.
        /// </returns>
        public static TComponentHub CreateInstance<TComponentHub>(IHttpServerContext httpServerContext, params object[] advancedParameters)
            where TComponentHub : class, IComponentHub
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = typeof(TComponentHub).GetConstructors(flags);

            if (constructors is not null)
            {
                foreach (var constructor in constructors.OrderByDescending(x => x.GetParameters().Length))
                {
                    // injection
                    var parameters = constructor.GetParameters();

                    var parameterValues = parameters.Select(parameter =>
                        parameter.ParameterType == typeof(IHttpServerContext) ? httpServerContext :

                        advancedParameters.Where(x => x.GetType() == parameter.ParameterType)
                                  .FirstOrDefault() ?? null
                    ).ToArray();

                    if (constructor.Invoke(parameterValues) is TComponentHub component)
                    {
                        return component;
                    }
                }
            }

            return Activator.CreateInstance(typeof(TComponentHub), advancedParameters) as TComponentHub;
        }

        /// <summary>
        /// Creates an instance of the specified component type with the provided context, 
        /// component hub advanced parameters.
        /// </summary>
        /// <typeparam name="TComponentManager">
        /// The type of the component manager, which must implement <see cref="IComponentManager"/>.
        /// </typeparam>
        /// <param name="componentType">
        /// The type of the component to create.
        /// </param>
        /// <param name="httpServerContext">
        /// The reference to the context of the host.
        /// </param>
        /// <param name="componentHub">
        /// The component hub to use for dependency injection.
        /// </param>
        /// <param name="advancedParameters">
        /// Additional parameters to pass to the component's constructor.
        /// </param>
        /// <returns>
        /// An instance of the specified component type.
        /// </returns>
        public static TComponentManager CreateInstance<TComponentManager>(Type componentType, IHttpServerContext httpServerContext, IComponentHub componentHub, params object[] advancedParameters)
            where TComponentManager : class, IComponentManager
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = componentType?.GetConstructors(flags);

            if (constructors is not null)
            {
                foreach (var constructor in constructors.OrderByDescending(x => x.GetParameters().Length))
                {
                    // injection
                    var parameters = constructor.GetParameters();
                    var properties = componentHub.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    var parameterValues = parameters.Select(parameter =>
                        parameter.ParameterType == typeof(IComponentHub) ? componentHub :
                        parameter.ParameterType == typeof(IHttpServerContext) ? httpServerContext :
                        properties.Where(x => x.PropertyType == parameter.ParameterType)
                                  .FirstOrDefault()?
                                  .GetValue(componentHub) ??
                        advancedParameters.Where(x => x.GetType() == parameter.ParameterType)
                                  .FirstOrDefault() ?? null
                    ).ToArray();

                    if (constructor.Invoke(parameterValues) is TComponentManager component)
                    {
                        return component;
                    }
                }
            }

            return Activator.CreateInstance(componentType) as TComponentManager;
        }

        /// <summary>
        /// Creates an instance of the specified component type with the provided context, 
        /// component hub advanced parameters.
        /// </summary>
        /// <typeparam name="TComponent">
        /// The type of the component manager, which must implement <see cref="IComponentManager"/>.
        /// </typeparam>
        /// <param name="httpServerContext">
        /// The reference to the context of the host.
        /// </param>
        /// <param name="componentHub">
        /// The component hub to use for dependency injection.
        /// </param>
        /// <param name="componentType">
        /// The type of the component to create.
        /// </param>
        /// <param name="advancedParameters">
        /// Additional parameters to pass to the component's constructor.
        /// </param>
        /// <returns>
        /// An instance of the specified component type.
        /// </returns>
        public static TComponent CreateInstance<TComponent>(IHttpServerContext httpServerContext, IComponentHub componentHub, Type componentType, params object[] advancedParameters)
            where TComponent : IComponent
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = componentType?.GetConstructors(flags);

            if (constructors is not null)
            {
                foreach (var constructor in constructors.OrderByDescending(x => x.GetParameters().Length))
                {
                    // injection
                    var parameters = constructor.GetParameters();
                    var properties = componentHub.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    var parameterValues = parameters.Select(parameter =>
                        parameter.ParameterType == typeof(IComponentHub) ? componentHub :
                        parameter.ParameterType == typeof(IHttpServerContext) ? httpServerContext :
                        properties.Where(x => x.PropertyType == parameter.ParameterType)
                                  .FirstOrDefault()?
                                  .GetValue(componentHub) ??
                        advancedParameters.Where(x => x.GetType() == parameter.ParameterType)
                                  .FirstOrDefault() ?? null
                    ).ToArray();

                    if (constructor.Invoke(parameterValues) is TComponent component)
                    {
                        return component;
                    }
                }
            }

            return (TComponent)Activator.CreateInstance(componentType);
        }

        /// <summary>
        /// Creates an instance of the specified component type with the provided context, 
        /// component hub advanced parameters.
        /// </summary>
        /// <typeparam name="TComponent">
        /// The type of the component, which must implement <see cref="IComponent"/>.
        /// </typeparam>
        /// <param name="httpServerContext">
        /// The reference to the context of the host.
        /// </param>
        /// <param name="componentHub">
        /// The component hub to use for dependency injection.
        /// </param>
        /// <param name="advancedParameters">
        /// Additional parameters to pass to the component's constructor.
        /// </param>
        /// <returns>
        /// An instance of the specified component type.
        /// </returns>
        public static TComponent CreateInstance<TComponent>(IHttpServerContext httpServerContext, IComponentHub componentHub, params object[] advancedParameters)
            where TComponent : class, IComponent
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var componentType = typeof(TComponent);
            var constructors = componentType?.GetConstructors(flags);

            if (constructors is not null)
            {
                foreach (var constructor in constructors.OrderByDescending(x => x.GetParameters().Length))
                {
                    // injection
                    var parameters = constructor.GetParameters();
                    var properties = componentHub.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    var parameterValues = parameters.Select(parameter =>
                        parameter.ParameterType == typeof(IComponentHub) ? componentHub :
                        parameter.ParameterType == typeof(IHttpServerContext) ? httpServerContext :
                        properties.Where(x => x.PropertyType == parameter.ParameterType)
                            .FirstOrDefault()?
                            .GetValue(componentHub) ??
                        advancedParameters.Where(x => x is not null)
                            .Where(x => x.GetType() == parameter.ParameterType)
                            .FirstOrDefault() ?? null
                    ).ToArray();

                    if (constructor.Invoke(parameterValues) is TComponent component)
                    {
                        return component;
                    }
                }
            }

            return Activator.CreateInstance(componentType) as TComponent;
        }

        /// <summary>
        /// Creates an instance of the specified component type with the provided context and 
        /// component hub and advanced parameters.
        /// </summary>
        /// <typeparam name="TComponent">
        /// The type of the component, which must implement <see cref="IComponent"/>.
        /// </typeparam>
        /// <typeparam name="TContext">
        /// The type of the context, which must implement <see cref="IContext"/>.
        /// </typeparam>
        /// <param name="componentType">
        /// The type of the component to create.
        /// </param>
        /// <param name="context">
        /// The context to pass to the component's constructor.
        /// </param>
        /// <param name="httpServerContext">
        /// The reference to the context of the host.
        /// </param>
        /// <param name="componentHub">
        /// The component hub to use for dependency injection.
        /// </param>
        /// <param name="advancedParameters">
        /// Additional parameters to pass to the component's constructor.
        /// </param>
        /// <returns>
        /// An instance of the specified component type.
        /// </returns>
        public static TComponent CreateInstance<TComponent, TContext>(Type componentType, TContext context, IHttpServerContext httpServerContext, IComponentHub componentHub, params object[] advancedParameters)
            where TComponent : class, IComponent
            where TContext : IContext
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = componentType?.GetConstructors(flags);

            if (constructors is not null)
            {
                foreach (var constructor in constructors.OrderByDescending(x => x.GetParameters().Length))
                {
                    // injection
                    var parameters = constructor.GetParameters();
                    var components = componentHub.Managers
                        .Select(x => new
                        {
                            type = x.GetType().GetInterfaces().FirstOrDefault(),
                            value = x
                        });
                    var contextIdProperty = context.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(x => x.PropertyType == typeof(IComponentId))
                        .FirstOrDefault();

                    var parameterValues = parameters.Select(parameter =>
                        parameter.ParameterType == typeof(IComponentHub) ? componentHub :
                        parameter.ParameterType == typeof(IHttpServerContext) ? httpServerContext :
                        parameter.ParameterType == typeof(TContext) ? context :
                        parameter.ParameterType == typeof(IComponentId) ? contextIdProperty?.GetValue(context) :
                        components.Where(x => x.type == parameter.ParameterType)
                            .Select(x => x.value)
                            .FirstOrDefault() ??
                        advancedParameters.Where(x =>
                                x.GetType() == parameter.ParameterType ||
                                (
                                    parameter.ParameterType == typeof(IApplicationContext) &&
                                    x.GetType().GetInterfaces().Any(x => x == typeof(IApplicationContext))
                                ) ||
                                (
                                    parameter.ParameterType == typeof(IPageContext) &&
                                    x.GetType().GetInterfaces().Any(x => x == typeof(IPageContext))
                                )
                            )
                            .FirstOrDefault() ?? null
                    ).ToArray();

                    if (constructor.Invoke(parameterValues) is TComponent component)
                    {
                        return component;
                    }
                }
            }

            return Activator.CreateInstance(componentType) as TComponent;
        }

        /// <summary>
        /// Creates an instance of the specified component type with the provided context and 
        /// component hub and advanced parameters.
        /// </summary>
        /// <typeparam name="TContext">
        /// The type of the context, which must implement <see cref="IContext"/>.
        /// </typeparam>
        /// <param name="componentType">
        /// The type of the component to create.
        /// </param>
        /// <param name="context">
        /// The context to pass to the component's constructor.
        /// </param>
        /// <param name="httpServerContext">
        /// The reference to the context of the host.
        /// </param>
        /// <param name="componentHub">
        /// The component hub to use for dependency injection.
        /// </param>
        /// <param name="advancedParameters">
        /// Additional parameters to pass to the component's constructor.
        /// </param>
        /// <returns>
        /// An instance of the specified component type.
        /// </returns>
        public static IComponent CreateInstance<TContext>(Type componentType, TContext context, IHttpServerContext httpServerContext, IComponentHub componentHub, params object[] advancedParameters)
            where TContext : IContext
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = componentType?.GetConstructors(flags);

            if (constructors is not null)
            {
                foreach (var constructor in constructors.OrderByDescending(x => x.GetParameters().Length))
                {
                    // injection
                    var parameters = constructor.GetParameters();
                    var properties = componentHub.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    var contextIdProperty = context.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(x => x.PropertyType == typeof(IComponentId))
                        .FirstOrDefault();

                    var parameterValues = parameters.Select(parameter =>
                        parameter.ParameterType == typeof(IComponentHub) ? componentHub :
                        parameter.ParameterType == typeof(IHttpServerContext) ? httpServerContext :
                        parameter.ParameterType == typeof(TContext) ? context :
                        parameter.ParameterType == typeof(IComponentId) ? contextIdProperty?.GetValue(context) :
                        properties.Where(x => x.PropertyType == parameter.ParameterType)
                                  .FirstOrDefault()?
                                  .GetValue(componentHub) ??
                        advancedParameters.Where(x => x.GetType() == parameter.ParameterType)
                                  .FirstOrDefault() ?? null
                    ).ToArray();

                    if (constructor.Invoke(parameterValues) is IComponent component)
                    {
                        return component;
                    }
                }
            }

            return Activator.CreateInstance(componentType) as IComponent;
        }
    }
}
