using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace T034.Tools.IO
{
    public interface IImageUploader
    {
        void Upload(HttpRequest request);
        IEnumerable<ImageModel> GetFiles();
    }

    public class ImageModel
    {
        public string Path { get; set; }
        public string Alt { get; set; }
    }
}
