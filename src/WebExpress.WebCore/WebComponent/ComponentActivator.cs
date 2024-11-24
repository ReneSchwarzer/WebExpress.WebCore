using System;
using System.Linq;
using System.Reflection;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebStatusPage;

namespace WebExpress.WebCore.WebComponent
{
    /// <summary>
    /// Provides methods to create instances of components with dependency injection.
    /// </summary>
    public static class ComponentActivator
    {
        /// <summary>
        /// Creates an instance of the specified response type with the component hub and advanced parameters.
        /// </summary>
        /// <typeparam name="T">The type of the response.</typeparam>
        /// <param name="responseType">The type of the response to create.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        /// <param name="componentHub">The component hub to use for dependency injection.</param>
        /// <param name="statusMessage">Additional parameter with a status message to pass to the response's constructor.</param>
        /// <param name="advancedParameters">Additional parameters to pass to the component's constructor.</param>
        /// <returns>An instance of the specified response type.</returns>
        public static T CreateInstance<T>(Type responseType, IHttpServerContext httpServerContext, IComponentHub componentHub, StatusMessage statusMessage, params object[] advancedParameters) where T : Response
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = responseType?.GetConstructors(flags);

            if (constructors != null)
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

                    if (constructor.Invoke(parameterValues) is T component)
                    {
                        return component;
                    }
                }
            }

            return Activator.CreateInstance(responseType) as T;
        }

        /// <summary>
        /// Creates an instance of the specified component type with the provided context, component hub advanced parameters.
        /// </summary>
        /// <typeparam name="T">The type of the component manager, which must implement <see cref="IComponentManager"/>.</typeparam>
        /// <param name="componentType">The type of the component to create.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        /// <param name="componentHub">The component hub to use for dependency injection.</param>
        /// <param name="advancedParameters">Additional parameters to pass to the component's constructor.</param>
        /// <returns>An instance of the specified component type.</returns>
        public static T CreateInstance<T>(Type componentType, IHttpServerContext httpServerContext, IComponentHub componentHub, params object[] advancedParameters) where T : class, IComponentManager
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = componentType?.GetConstructors(flags);

            if (constructors != null)
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

                    if (constructor.Invoke(parameterValues) is T component)
                    {
                        return component;
                    }
                }
            }

            return Activator.CreateInstance(componentType) as T;
        }

        /// <summary>
        /// Creates an instance of the specified component type with the provided context, component hub advanced parameters.
        /// </summary>
        /// <typeparam name="T">The type of the component manager, which must implement <see cref="IComponentManager"/>.</typeparam>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        /// <param name="componentHub">The component hub to use for dependency injection.</param>
        /// <param name="componentType">The type of the component to create.</param>
        /// <param name="advancedParameters">Additional parameters to pass to the component's constructor.</param>
        /// <returns>An instance of the specified component type.</returns>
        public static T CreateInstance<T>(IHttpServerContext httpServerContext, IComponentHub componentHub, Type componentType, params object[] advancedParameters) where T : IComponent
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = componentType?.GetConstructors(flags);

            if (constructors != null)
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

                    if (constructor.Invoke(parameterValues) is T component)
                    {
                        return component;
                    }
                }
            }

            return (T)Activator.CreateInstance(componentType);
        }

        /// <summary>
        /// Creates an instance of the specified component type with the provided context, component hub advanced parameters.
        /// </summary>
        /// <typeparam name="T">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        /// <param name="componentHub">The component hub to use for dependency injection.</param>
        /// <param name="advancedParameters">Additional parameters to pass to the component's constructor.</param>
        /// <returns>An instance of the specified component type.</returns>
        public static T CreateInstance<T>(IHttpServerContext httpServerContext, IComponentHub componentHub, params object[] advancedParameters) where T : class, IComponent
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var componentType = typeof(T);
            var constructors = componentType?.GetConstructors(flags);

            if (constructors != null)
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
                        advancedParameters.Where(x => x != null)
                            .Where(x => x.GetType() == parameter.ParameterType)
                            .FirstOrDefault() ?? null
                    ).ToArray();

                    if (constructor.Invoke(parameterValues) is T component)
                    {
                        return component;
                    }
                }
            }

            return Activator.CreateInstance(componentType) as T;
        }

        /// <summary>
        /// Creates an instance of the specified component type with the provided context and component hub and advanced parameters.
        /// </summary>
        /// <typeparam name="T">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
        /// <typeparam name="C">The type of the context, which must implement <see cref="IContext"/>.</typeparam>
        /// <param name="componentType">The type of the component to create.</param>
        /// <param name="context">The context to pass to the component's constructor.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        /// <param name="componentHub">The component hub to use for dependency injection.</param>
        /// <param name="advancedParameters">Additional parameters to pass to the component's constructor.</param>
        /// <returns>An instance of the specified component type.</returns>
        public static T CreateInstance<T, C>(Type componentType, C context, IHttpServerContext httpServerContext, IComponentHub componentHub, params object[] advancedParameters) where T : class, IComponent where C : IContext
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = componentType?.GetConstructors(flags);

            if (constructors != null)
            {
                foreach (var constructor in constructors.OrderByDescending(x => x.GetParameters().Length))
                {
                    // injection
                    var parameters = constructor.GetParameters();
                    var properties = componentHub.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    var parameterValues = parameters.Select(parameter =>
                        parameter.ParameterType == typeof(IComponentHub) ? componentHub :
                        parameter.ParameterType == typeof(IHttpServerContext) ? httpServerContext :
                        parameter.ParameterType == typeof(C) ? context :
                        properties.Where(x => x.PropertyType == parameter.ParameterType)
                                  .FirstOrDefault()?
                                  .GetValue(componentHub) ??
                        advancedParameters.Where(x => x.GetType() == parameter.ParameterType)
                                  .FirstOrDefault() ?? null
                    ).ToArray();

                    if (constructor.Invoke(parameterValues) is T component)
                    {
                        return component;
                    }
                }
            }

            return Activator.CreateInstance(componentType) as T;
        }

        /// <summary>
        /// Creates an instance of the specified component type with the provided context and component hub and advanced parameters.
        /// </summary>
        /// <typeparam name="T">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
        /// <typeparam name="C">The type of the context, which must implement <see cref="IContext"/>.</typeparam>
        /// <param name="componentType">The type of the component to create.</param>
        /// <param name="context">The context to pass to the component's constructor.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        /// <param name="componentHub">The component hub to use for dependency injection.</param>
        /// <param name="advancedParameters">Additional parameters to pass to the component's constructor.</param>
        /// <returns>An instance of the specified component type.</returns>
        public static IComponent CreateInstance<C>(Type componentType, C context, IHttpServerContext httpServerContext, IComponentHub componentHub, params object[] advancedParameters) where C : IContext
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = componentType?.GetConstructors(flags);

            if (constructors != null)
            {
                foreach (var constructor in constructors.OrderByDescending(x => x.GetParameters().Length))
                {
                    // injection
                    var parameters = constructor.GetParameters();
                    var properties = componentHub.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    var parameterValues = parameters.Select(parameter =>
                        parameter.ParameterType == typeof(IComponentHub) ? componentHub :
                        parameter.ParameterType == typeof(IHttpServerContext) ? httpServerContext :
                        parameter.ParameterType == typeof(C) ? context :
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
