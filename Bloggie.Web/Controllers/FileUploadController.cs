using System.Net;
using Bloggie.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        [HttpPost]
        [Route("FileUpload")]
        public async Task<ActionResult> UploadToCloudinaryAsync(IFormFile files)
        {
            var uploadedDocUrl = await Commonservice.UploadToCloudinaryAsync(files);
            if (uploadedDocUrl == null) return Problem("Something went wrong!", null, (int)HttpStatusCode.BadRequest);
            return new JsonResult(new { link = uploadedDocUrl });
        }
    }
}
