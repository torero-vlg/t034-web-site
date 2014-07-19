using System.Web.Mvc;
using Db;
using Db.Entity;

namespace T034.Controllers
{
    public class DbController : Controller
    {
        public static AbstractDbFactory DbFactory;

        public ActionResult Index()
        {
            DbFactory = new NhDbFactory(MvcApplication.ConnectionString);

            var db = DbFactory.CreateBaseDb();

            var list = db.Select<News>();

            return View("NewsList", list);
        }

    }
}
