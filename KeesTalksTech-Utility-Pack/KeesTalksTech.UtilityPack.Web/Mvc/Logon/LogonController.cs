using KeesTalksTech.Utilities.Web.Authentication;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;

namespace KeesTalksTech.Utilities.Web.Mvc.Logon
{
    /// <summary>
    /// Controller that helps with Forms Authentication.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class LogonController : Controller
    {
        /// <summary>
        /// Opens the logon page.
        /// </summary>
        /// <returns>The action result.</returns>
        public virtual ActionResult Index()
        {
            return View(new LogonModel());
        }

        /// <summary>
        /// Logs the user in.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public virtual ActionResult Index(LogonModel model)
        {
            Authenticate(model);
            return View(model);
        }

        /// <summary>
        /// Authenticates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="redirect">if set to <c>true</c> a redirect will be performed when the user is logged on.</param>
        /// <returns><c>true</c> if the model could be authenticated; otherwise <c>false</c>.</returns>
        protected virtual bool Authenticate(LogonModel model, bool redirect = true)
        {
            bool authenticated = FormsAuthentication.Authenticate(model.UserName, model.Password);

            if (!authenticated)
            {
                return false;
            }

            var user = new GenericIdentity(model.UserName);
            var principal = new SimpleConfigurationPrincipal(user);
            SimpleConfigurationPrincipal.SetAuthenticatedPrincipal(principal);

            if (redirect)
            {
                FormsAuthentication.RedirectFromLoginPage(model.UserName, false);
            }

            return true;
        }

        /// <summary>
        /// Logs the user off.
        /// </summary>
        /// <returns>The action result.</returns>
        public virtual ActionResult Logoff()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Index");
        }
    }
}