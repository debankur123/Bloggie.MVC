using Bloggie.Web.Models.Request;
using Bloggie.Web.Repository.DatabaseContext;
using Bloggie.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Bloggie.Web.Controllers;
public class BlogPostController(BlogService service, BloggieWebContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> CreateBlogPost(long? TagId=0)
    {
        var tags = await service.GetTags(TagId);
        var viewModel = new BlogPostRequest
        {
            Tags = tags.Select(x => new SelectListItem
            {
                Text = x.TagName,
                Value = x.TagId.ToString()
            })
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlogPost(BlogPostRequest request)
    {
        return RedirectToAction("CreateBlogPost");
    }
}