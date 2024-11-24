using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebIdentity.Model
{
    /// <summary>
    /// The identity role directory.
    /// </summary>
    internal class IdentityRoleDictionary : Dictionary<IPluginContext, Dictionary<IApplicationContext, IList<IdentityRoleItem>>>
    {
        /// <summary>
        /// Adds a role item to the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="roleItem">The role item.</param>
        /// <returns>True if the role item was successfully added, false if an item with the same role class already exists.</returns>
        public bool AddRoleItem(IPluginContext pluginContext, IApplicationContext applicationContext, IdentityRoleItem roleItem)
        {
            var type = roleItem.RoleClass;

            if (!typeof(IIdentityRole).IsAssignableFrom(type))
            {
                return false;
            }

            if (!TryGetValue(pluginContext, out var appContextDict))
            {
                appContextDict = new Dictionary<IApplicationContext, IList<IdentityRoleItem>>();
                this[pluginContext] = appContextDict;
            }

            if (!appContextDict.TryGetValue(applicationContext, out var roleList))
            {
                roleList = new List<IdentityRoleItem>();
                appContextDict[applicationContext] = roleList;
            }

            if (roleList.Any(x => x.RoleClass == type))
            {
                return false; // an item with the same role class already exists
            }

            roleList.Add(roleItem);

            return true;
        }

        /// <summary>
        /// Removes a role item from the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        public void RemoveRoleItem<T>(IPluginContext pluginContext, IApplicationContext applicationContext) where T : IIdentityRole
        {
            var type = typeof(T);

            if (ContainsKey(pluginContext))
            {
                var appContextDict = this[pluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var roleList = appContextDict[applicationContext];

                    var itemToRemove = roleList.FirstOrDefault(x => x.RoleClass == type);
                    if (itemToRemove != null)
                    {
                        roleList.Remove(itemToRemove);

                        if (roleList.Count == 0)
                        {
                            appContextDict.Remove(applicationContext);

                            if (appContextDict.Count == 0)
                            {
                                Remove(pluginContext);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the role items from the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of the role.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An IEnumerable of role items</returns>
        public IEnumerable<IdentityRoleItem> GetRoleItems<T>(IApplicationContext applicationContext) where T : IIdentityRole
        {
            return GetRoleItems(applicationContext, typeof(T));
        }

        /// <summary>
        /// Returns the role items from the dictionary.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <typeparam name="roleType">The type of the role.</typeparam>
        /// <returns>An IEnumerable of role items</returns>
        public IEnumerable<IdentityRoleItem> GetRoleItems(IApplicationContext applicationContext, Type roleType)
        {
            if (!typeof(IIdentityRole).IsAssignableFrom(roleType))
            {
                return Enumerable.Empty<IdentityRoleItem>();
            }

            if (ContainsKey(applicationContext?.PluginContext))
            {
                var appContextDict = this[applicationContext?.PluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var roleList = appContextDict[applicationContext];

                    return roleList.Where(x => x.RoleClass == roleType);
                }
            }

            return Enumerable.Empty<IdentityRoleItem>();
        }

        /// <summary>
        /// Returns all role contexts for a given plugin context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <returns>An IEnumerable of role contexts.</returns>
        public IEnumerable<IIdentityRoleContext> GetRoleContexts(IPluginContext pluginContext)
        {
            return this.Where(x => x.Key == pluginContext)
                .SelectMany(x => x.Value)
                .SelectMany(x => x.Value)
                .Select(x => x.RoleContext);
        }

        /// <summary>
        /// Returns all role contexts for a given application context.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An IEnumerable of role contexts.</returns>
        public IEnumerable<IIdentityRoleContext> GetRoleContexts(IApplicationContext applicationContext)
        {
            return Values.SelectMany(x => x)
                .Where(x => x.Key == applicationContext)
                .SelectMany(entry => entry.Value)
                .Select(x => x.RoleContext);
        }
    }
}
