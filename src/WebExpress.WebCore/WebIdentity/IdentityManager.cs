using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebIdentity.Model;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebSession.Model;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Management of identities (users).
    /// </summary>
    public class IdentityManager : IIdentityManager
    {
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;
        private readonly IdentityPermissionDictionary _permissionDictionary = [];
        private readonly IdentityPolicyDictionary _policyDictionary = [];

        /// <summary>
        /// Returns all permissions.
        /// </summary>
        public IEnumerable<IIdentityPermissionContext> Permissions => _permissionDictionary.Values
            .SelectMany(x => x.Values)
            .SelectMany(x => x)
            .Select(x => x.PermissionContext);

        /// <summary>
        /// Returns all policies.
        /// </summary>
        public IEnumerable<IIdentityPolicyContext> Policies => _policyDictionary.Values
            .SelectMany(x => x.Values)
            .SelectMany(x => x)
            .Select(x => x.PolicyContext);

        /// <summary>
        /// Returns all identities.
        /// </summary>
        public IEnumerable<IIdentity> Identities => [];

        /// <summary>
        /// Returns the current signed-in identity.
        /// </summary>
        public IIdentity CurrentIdentity { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used via Reflection.")]
        private IdentityManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication += OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication += OnRemoveApplication;

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate
                (
                    "webexpress.webcore:identitymanager.initialization"
                )
            );
        }

        /// <summary>
        /// Discovers and binds jobs to an application.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin whose jobs are to be associated.</param>
        private void Register(IPluginContext pluginContext)
        {
            if (_permissionDictionary.ContainsKey(pluginContext))
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
                if (_permissionDictionary.TryGetValue(pluginContext, out var appDict) && appDict.ContainsKey(applicationContext))
                {
                    continue;
                }

                Register(pluginContext, [applicationContext]);
            }
        }

        /// <summary>
        /// Registers policies and ientities for a given plugin and application context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContexts">The application context (optional).</param>
        private void Register(IPluginContext pluginContext, IEnumerable<IApplicationContext> applicationContexts)
        {
            var assembly = pluginContext?.Assembly;

            // permissions
            foreach (var permissionType in assembly.GetTypes().Where
                (
                    x => x.IsClass == true &&
                    x.IsSealed &&
                    x.IsPublic &&
                    (
                        x.GetInterface(typeof(IIdentityPermission).Name) != null
                    )
                ))
            {
                var id = new ComponentId(permissionType.FullName);
                var policyTypes = new List<Type>();

                foreach (var customAttribute in permissionType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IPolicyAttribute))))
                {
                    if (customAttribute.AttributeType.Name == typeof(PolicyAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(PolicyAttribute<>).Namespace)
                    {
                        var type = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault();
                        if (type != null && !policyTypes.Contains(type))
                        {
                            policyTypes.Add(type);
                        }
                    }
                }

                // assign the event to existing applications
                foreach (var applicationContext in applicationContexts)
                {
                    var permissionContext = new IdentityPermissionContext()
                    {
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        PermissionId = id,
                        Permission = permissionType
                    };

                    if (_permissionDictionary.AddPermissionItem
                    (
                        pluginContext,
                        applicationContext,
                        new IdentityPermissionItem(_componentHub, _httpServerContext, pluginContext, applicationContext, permissionType, permissionContext, policyTypes)
                    ))
                    {
                        _httpServerContext.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress.webcore:identitymanager.registerpermission",
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
                                "webexpress.webcore:identitymanager.duplicatepermission",
                                id,
                                applicationContext.ApplicationId
                            )
                        );
                    }
                }
            }

            // policies
            foreach (var policyType in assembly.GetTypes().Where
                (
                    x => x.IsClass == true &&
                    x.IsSealed &&
                    x.IsPublic &&
                    (
                        x.GetInterface(typeof(IIdentityPolicy).Name) != null
                    )
                ))
            {
                var id = new ComponentId(policyType.FullName);
                var permissionTypes = new List<Type>();

                foreach (var customAttribute in policyType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IPermissionAttribute))))
                {
                    if (customAttribute.AttributeType.Name == typeof(PermissionAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(PermissionAttribute<>).Namespace)
                    {
                        var type = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault();
                        if (type != null && !permissionTypes.Contains(type))
                        {
                            permissionTypes.Add(type);
                        }
                    }
                }

                // assign the event to existing applications
                foreach (var applicationContext in applicationContexts)
                {
                    var policyContext = new IdentityPolicyContext()
                    {
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        PolicyId = id
                    };

                    if (_policyDictionary.AddPolicyItem
                    (
                        pluginContext,
                        applicationContext,
                        new IdentityPolicyItem(_componentHub, _httpServerContext, pluginContext, applicationContext, policyType, policyContext, permissionTypes)
                    ))
                    {
                        _httpServerContext.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress.webcore:identitymanager.registerpolicy",
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
                                "webexpress.webcore:identitymanager.duplicatepolicy",
                                id,
                                applicationContext.ApplicationId
                            )
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Removes all policies and permissions of an plugin.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the identities to remove.</param>
        internal void Remove(IPluginContext pluginContext)
        {
            // permissions
            if (_permissionDictionary.TryGetValue(pluginContext, out var permissionValue))
            {
                foreach (var permissionItem in permissionValue
                    .SelectMany(x => x.Value))
                {
                    permissionItem.Dispose();
                }

                _permissionDictionary.Remove(pluginContext);
            }

            // policies
            if (_policyDictionary.TryGetValue(pluginContext, out var policyValue))
            {
                foreach (var permissionItem in policyValue
                    .SelectMany(x => x.Value))
                {
                    permissionItem.Dispose();
                }

                _policyDictionary.Remove(pluginContext);
            }
        }

        /// <summary>
        /// Removes all policies and permissions of an application.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the identities to remove.</param>
        internal void Remove(IApplicationContext applicationContext)
        {
            if (applicationContext == null)
            {
                return;
            }

            // permissions
            foreach (var pluginDict in _permissionDictionary.Values)
            {
                foreach (var appDict in pluginDict.Where(x => x.Key == applicationContext).Select(x => x.Value))
                {
                    foreach (var permissionItem in appDict)
                    {
                        permissionItem.Dispose();
                    }
                }

                pluginDict.Remove(applicationContext);
            }

            // policies
            foreach (var pluginDict in _policyDictionary.Values)
            {
                foreach (var appDict in pluginDict.Where(x => x.Key == applicationContext).Select(x => x.Value))
                {
                    foreach (var policyItem in appDict)
                    {
                        policyItem.Dispose();
                    }
                }

                pluginDict.Remove(applicationContext);
            }
        }

        /// <summary>
        /// Raises the event when an plugin is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the plugin being added.</param>
        private void OnAddPlugin(object sender, IPluginContext e)
        {
            Register(e);
        }

        /// <summary>  
        /// Raises the event when a plugin is removed.  
        /// </summary>  
        /// <param name="sender">The source of the event.</param>  
        /// <param name="e">The context of the plugin being removed.</param>  
        private void OnRemovePlugin(object sender, IPluginContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Raises the event when an application is removed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being removed.</param>
        private void OnRemoveApplication(object sender, IApplicationContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Raises the event when an application is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being added.</param>
        private void OnAddApplication(object sender, IApplicationContext e)
        {
            Register(e);
        }

        /// <summary>
        /// Login an identity.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="identity">The identity.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool Login(Request request, IIdentity identity, SecureString password)
        {
            if (identity?.PasswordHash == ComputeHash(password))
            {
                var session = _componentHub.SessionManager.GetSession(request);
                var authentification = session.GetOrCreateProperty<SessionPropertyAuthentification>(identity);

                if (authentification.Identity != identity)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Logout an identity.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Logout(Request request)
        {
            var session = _componentHub.SessionManager.GetSession(request);
            session.RemoveProperty<SessionPropertyAuthentification>();
        }

        /// <summary>
        /// Returns the current signed-in identity based on the provided request.
        /// </summary>
        /// <param name="request">The request to get the current identity for.</param>
        /// <returns>The current signed-in identity.</returns>
        public IIdentity GetCurrentIdentity(Request request)
        {
            var session = _componentHub.SessionManager.GetSession(request);
            var authentification = session.GetProperty<SessionPropertyAuthentification>();

            return authentification?.Identity;
        }

        /// <summary>
        /// Checks if the specified identity has the given permission.
        /// </summary>
        /// <typeparam name="T">The type of the identity permission.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="identity">The identity to check.</param>
        /// <returns>True if the identity has the permission, false otherwise.</returns>
        public bool CheckAccess<T>(IApplicationContext applicationContext, IIdentity identity) where T : IIdentityPermission
        {
            return CheckAccess(applicationContext, identity, typeof(T));
        }

        /// <summary>
        /// Checks if the specified identity has the given permission.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="identity">The identity to check.</param>
        /// <param name="permission">The permission to check for.</param>
        /// <returns>True if the identity has the permission, false otherwise.</returns>
        public bool CheckAccess(IApplicationContext applicationContext, IIdentity identity, Type permission)
        {
            foreach (var group in identity?.Groups ?? [])
            {
                if (CheckAccess(applicationContext, group, permission))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the specified identity group has the given permission.
        /// </summary>
        /// <typeparam name="T">The type of the identity permission.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="group">The identity group to check.</param>
        /// <returns>True if the identity group has the permission, false otherwise.</returns>
        public bool CheckAccess<T>(IApplicationContext applicationContext, IIdentityGroup group) where T : IIdentityPermission
        {
            return CheckAccess(applicationContext, group, typeof(T));
        }

        /// <summary>
        /// Checks if the specified identity group has the given permission.
        /// </summary>
        /// <param name="group">The identity group to check.</param>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="permission">The permission to check for.</param>
        /// <returns>True if the identity group has the permission, false otherwise.</returns>
        public bool CheckAccess(IApplicationContext applicationContext, IIdentityGroup group, Type permission)
        {
            foreach (var policy in group?.Policies ?? [])
            {
                if (CheckAccess(applicationContext, policy, permission))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the specified identity policy has the given permission.
        /// </summary>
        /// <typeparam name="R">The type of the identity policy.</typeparam>
        /// <typeparam name="P">The type of the identity permission.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>True if the identity policy has the permission, false otherwise.</returns>
        public bool CheckAccess<R, P>(IApplicationContext applicationContext) where R : IIdentityPolicy where P : IIdentityPermission
        {
            return CheckAccess(applicationContext, typeof(R), typeof(P));
        }
        /// <summary>
        /// Checks if the specified identity policy has the given permission.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="policyType">The identity policy to check.</param>
        /// <param name="permissionType">The permission to check for.</param>
        /// <returns>True if the identity policy has the permission, false otherwise.</returns>
        public bool CheckAccess(IApplicationContext applicationContext, Type policyType, Type permissionType)
        {
            return CheckAccess(applicationContext, policyType.FullName.ToLower(), permissionType);
        }

        /// <summary>
        /// Checks if the specified identity policy has the given permission.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="policyName">The identity policy to check.</param>
        /// <param name="permissionType">The permission to check for.</param>
        /// <returns>True if the identity policy has the permission, false otherwise.</returns>
        private bool CheckAccess(IApplicationContext applicationContext, string policyName, Type permissionType)
        {
            // policies to permissions
            var policies = _policyDictionary.Values.SelectMany(x => x)
                .Where(x => x.Key == applicationContext)
                .SelectMany(entry => entry.Value);

            foreach (var policy in policies.Where(x => x.PolicyClass.FullName.Equals(policyName, StringComparison.CurrentCultureIgnoreCase)))
            {
                if (policy.Permissions.Contains(permissionType))
                {
                    return true;
                }
            }

            // permissions to policies
            var permissions = _permissionDictionary.Values.SelectMany(x => x)
                .Where(x => x.Key == applicationContext)
                .SelectMany(entry => entry.Value);

            foreach (var permission in permissions.Where(x => x.PermissionClass == permissionType))
            {
                if (permission.Policies.Any(x => x.FullName.Equals(policyName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Computes the SHA-256 hash of the input string.
        /// </summary>
        /// <param name="input">The input string to hash.</param>
        /// <returns>The computed hash as a hexadecimal string.</returns>
        public static string ComputeHash(SecureString input)
        {
            var bstr = IntPtr.Zero;
            try
            {
                bstr = Marshal.SecureStringToBSTR(input);
                var length = Marshal.ReadInt32(bstr, -4);
                var bytes = new byte[length];
                Marshal.Copy(bstr, bytes, 0, length);
                var hashBytes = SHA256.HashData(bytes);
                var builder = new StringBuilder();

                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
            finally
            {
                if (bstr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(bstr);
                }
            }
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
