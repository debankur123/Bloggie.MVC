using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

public class AdminBlogPost : Controller
{
    public IActionResult CreateBlogPost()
    {
        return View("");
    }
}