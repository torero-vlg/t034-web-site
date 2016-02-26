using System.Web.Mvc;
using Db.DataAccess;
using Ninject;
using NLog;
using T034.Repository;

namespace T034.Controllers
{
    public class BaseController : Controller
    {
        [Inject]
        public IBaseDb Db { get; set; }

        [Inject]
        public IRepository Repository { get; set; }

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();
    }
}