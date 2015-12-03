using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using T034.ViewModel;

namespace T034.Controllers
{
    public class UploadController : BaseController
    {
        public void UploadNow(HttpPostedFileWrapper upload)
        {
            if (upload != null)
            {
                var imageName = upload.FileName;
                var path = Path.Combine(Server.MapPath("~/Upload/Images"), imageName);//TODO перенести путь в config
                upload.SaveAs(path);
            }
        }

        public ActionResult UploadPartial()
        {
            var appData = Server.MapPath("~/Upload/Images");//TODO перенести путь в config
            var images = Directory.GetFiles(appData).Select(x => new ImageViewModel
            {
                Url = Url.Content("/Upload/Images/" + Path.GetFileName(x))//TODO перенести путь в config
            });
            return View(images);
        }
    }
}
