using KeesTalksTech.Utilities.Web.Authentication;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;

namespace KeesTalksTech.Utilities.Web.Mvc.Logon
{
    public class LogonController : Controller
    {
        [Route("Logon")]
        public virtual ActionResult Index()
        {
            return View(new LogonModel());
        }

        [HttpPost]
        public virtual ActionResult Index(LogonModel model)
        {
            Authenticate(model);
            return View(model);
        }

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

        [Route("Logoff")]
        public virtual ActionResult Logoff()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Index");
        }
    }
}