using System;
using System.Linq;
using System.Reflection;

namespace WebExpress.WebCore.WebComponent
{
    /// <summary>
    /// Provides methods to create instances of components with dependency injection.
    /// </summary>
    public static class ComponentActivator
    {
        /// <summary>
        /// Creates an instance of the specified component type with the provided context and component manager.
        /// </summary>
        /// <typeparam name="T">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
        /// <param name="componentType">The type of the component to create.</param>
        /// <param name="componentManager">The component manager to use for dependency injection.</param>
        /// <returns>An instance of the specified component type.</returns>
        public static T CreateInstance<T>(Type componentType, ComponentHub componentManager) where T : class, IComponentManager
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = componentType?.GetConstructors(flags);

            if (constructors != null)
            {
                foreach (var constructor in constructors)
                {
                    // injection
                    var parameters = constructor.GetParameters();
                    var properties = componentManager.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    var parameterValues = parameters.Select(parameter =>
                        parameter.ParameterType == typeof(IComponentHub) ? componentManager :
                        parameter.ParameterType == typeof(IHttpServerContext) ? componentManager.HttpServerContext :
                        properties.Where(x => x.PropertyType == parameter.ParameterType)
                                  .FirstOrDefault()?
                                  .GetValue(componentManager) ?? null
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
        /// Creates an instance of the specified component type with the provided context and component manager.
        /// </summary>
        /// <typeparam name="T">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
        /// <typeparam name="C">The type of the context, which must implement <see cref="IContext"/>.</typeparam>
        /// <param name="componentType">The type of the component to create.</param>
        /// <param name="context">The context to pass to the component's constructor.</param>
        /// <param name="componentManager">The component manager to use for dependency injection.</param>
        /// <param name="advancedParameters">Additional parameters to pass to the component's constructor.</param>
        /// <returns>An instance of the specified component type.</returns>
        public static T CreateInstance<T, C>(Type componentType, C context, IComponentHub componentManager, params object[] advancedParameters) where T : class, IComponent where C : IContext
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var constructors = componentType?.GetConstructors(flags);

            if (constructors != null)
            {
                foreach (var constructor in constructors)
                {
                    // injection
                    var parameters = constructor.GetParameters();
                    var properties = componentManager.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    var parameterValues = parameters.Select(parameter =>
                        parameter.ParameterType == typeof(IComponentHub) ? componentManager :
                        parameter.ParameterType == typeof(IHttpServerContext) ? componentManager.HttpServerContext :
                        parameter.ParameterType == typeof(C) ? context :
                        properties.Where(x => x.PropertyType == parameter.ParameterType)
                                  .FirstOrDefault()?
                                  .GetValue(componentManager) ??
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
    }
}
