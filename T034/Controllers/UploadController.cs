using System.Linq;
using Microsoft.AspNetCore.Mvc;
using T034.ViewModel;
using T034.Core.DataAccess;
using T034.Tools.IO;
using AutoMapper;

namespace T034.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IImageUploader _imageUploader;

        public UploadController(
            IImageUploader imageUploader,
            IBaseDb db, IMapper mapper) 
            : base(db, mapper)
        {
            _imageUploader = imageUploader;
        }

        public void UploadNow()
        {
            _imageUploader.Upload(Request);
        }

        public ActionResult UploadPartial()
        {
            var images = _imageUploader.GetFiles().Select(image => new ImageViewModel { Alt = image.Alt, Url = Url.Content("/" + image.Path) });
            return View(images);
        }
    }
}
