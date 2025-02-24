using Bloggie.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    [HttpPost]
    [Route("FileUpload")]
    public async Task<IActionResult> UploadToCloudinaryAsync([FromForm] IFormFile file)
    {
        if (file == null)
        {
            return BadRequest("No file uploaded.");
        }
        try
        {
            var uploadedDocUrl = await Commonservice.UploadToCloudinaryAsync(file);
            return new JsonResult(new { link = uploadedDocUrl });
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Problem($"Error occurred: {ex.Message}", null, (int)HttpStatusCode.InternalServerError);
        }
        catch (Exception ex)
        {
            return Problem($"Unexpected error occurred: {ex.Message}", null, (int)HttpStatusCode.InternalServerError);
        }
    }
}
