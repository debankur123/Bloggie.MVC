using Bloggie.Web.Models;
using Bloggie.Web.Models.Response;
using Bloggie.Web.Repository.DatabaseContext;
using Bloggie.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bloggie.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public BlogService Service { get; }
        private readonly BloggieWebContext context;

        public HomeController(ILogger<HomeController> logger, BlogService service, BloggieWebContext context)
        {
            this.context = context;
            Service = service;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index(long? BlogId = 0)
        {
            var blogDetails = await BlogService.GetBlogDetails(BlogId);
            return View((List<BlogDetailsResponse>)blogDetails);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> ViewBlogDetails(long BlogId)
        {
            var blogDetailsList = await BlogService.ViewBlogDetails(BlogId);
            var viewBlogDetails = blogDetailsList.FirstOrDefault();
            if (viewBlogDetails == null)
            {
                return NotFound();
            }
            return View("ViewBlogDetails", viewBlogDetails);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
