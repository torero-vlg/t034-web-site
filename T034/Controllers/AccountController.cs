using System.Linq;
using System.Web.Mvc;
using OAuth2;
using T034.ViewModel;

namespace T034.Controllers
{

    public class AccountController : BaseController
    {
        public AccountController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }

        /// <summary>
        /// Renders home page with login link.
        /// </summary>
        public ActionResult Logon()
        {
            var clients = AuthorizationRoot.Clients.Select(client => new LoginInfoModel
            {
                ProviderName = client.Name
            });
            return View(new LogonViewModel { Clients = clients});
        }

        /// <summary>
        /// Redirect to login url of selected provider.
        /// </summary>        
        public RedirectResult Login(string providerName)
        {
            ProviderName = providerName;
            return new RedirectResult(GetClient().GetLoginLinkUri());
        }

        /// <summary>
        /// Renders information received from authentication service.
        /// </summary>
        public ActionResult Auth()
        {
            return View(GetClient().GetUserInfo(Request.QueryString));
        }
    }
}
