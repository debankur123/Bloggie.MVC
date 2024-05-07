using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

public class BlogPostController : Controller
{
    public IActionResult CreateBlogPost()
    {
        return View();
    }
}