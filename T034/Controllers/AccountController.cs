using System.Linq;
using System.Web.Mvc;
using OAuth2;
using OAuth2.Client;
using T034.ViewModel;

namespace T034.Controllers
{

    public class AccountController : Controller
    {
        private readonly AuthorizationRoot _authorizationRoot;

        private const string ProviderNameKey = "providerName";

        private string ProviderName
        {
            get { return (string)Session[ProviderNameKey]; }
            set { Session[ProviderNameKey] = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="authorizationRoot">The authorization manager.</param>
        public AccountController(AuthorizationRoot authorizationRoot)
        {
            _authorizationRoot = authorizationRoot;
        }

        /// <summary>
        /// Renders home page with login link.
        /// </summary>
        public ActionResult Logon()
        {
            var clients = _authorizationRoot.Clients.Select(client => new LoginInfoModel
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

        private IClient GetClient()
        {
            return _authorizationRoot.Clients.First(c => c.Name == ProviderName);
        }
    }
}
