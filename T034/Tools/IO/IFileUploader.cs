using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace T034.Tools.IO
{
    public interface IFileUploader
    {
        ViewDataUploadFilesResult UploadPartialFile(HttpRequest request);
        IEnumerable<ViewDataUploadFilesResult> UploadWholeFile(HttpRequest request);
    }
}
