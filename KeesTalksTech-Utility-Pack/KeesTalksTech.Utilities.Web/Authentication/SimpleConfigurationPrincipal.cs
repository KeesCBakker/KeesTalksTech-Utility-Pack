using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace KeesTalksTech.Utilities.Web.Authentication
{
    /// <summary>
    /// Principal that looks for appSetting "roles:{userName}" for the roles. Roles should be separated with pipes.
    /// </summary>
    /// <seealso cref="System.Security.Principal.IPrincipal" />
    [DebuggerDisplay("{Identity.Name")]
    public class SimpleConfigurationPrincipal : IPrincipal
    {
        HashSet<string> _roles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleConfigurationPrincipal"/> class.
        /// </summary>
        /// <param name="identity">The identity.</param>
        public SimpleConfigurationPrincipal(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            this.Identity = identity;

            string roleSetting = $"roles:{identity.Name}";
            var roles = ConfigurationManager.AppSettings[roleSetting];

            if (!string.IsNullOrEmpty(roles))
            {
                foreach (var role in roles?.Split('|'))
                {
                    _roles.Add(role);
                }
            }
        }

        /// <summary>
        /// Gets the identity.
        /// </summary>
        /// <value>
        /// The identity.
        /// </value>
        public IIdentity Identity
        {
            get;
            private set;
        }

        /// <summary>
        /// Determines whether the user has the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns><c>true</c> if the user has the role; otherwise <c>false</c>.</returns>
        public bool IsInRole(string role)
        {
            return _roles.Contains(role);
        }

        /// <summary>
        /// Sets the authenticated principal.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public static void SetAuthenticatedPrincipal(IPrincipal principal)
        {
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated || String.IsNullOrEmpty(principal.Identity.Name))
            {
                return;
            }

            if (!(principal is SimpleConfigurationPrincipal))
            {
                principal = new SimpleConfigurationPrincipal(principal.Identity);
            }

            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }

            Thread.CurrentPrincipal = principal;
        }
    }
}