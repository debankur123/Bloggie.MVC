using Bloggie.Web.Models.Entity;
using Bloggie.Web.Models.Request;
using Bloggie.Web.Repository.DatabaseContext;
using Bloggie.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BlogService _service;
        private readonly BloggieWebContext _dbcontext;
        public AdminTagsController(BlogService service,BloggieWebContext context)
        {
            _service = service;
            _dbcontext = context;
        }
        [HttpGet]
        public IActionResult CreateTags()
        {
            return View();
        }
        [HttpPost]
        [ActionName("CreateTags")]
        public async Task<ActionResult> CreateTags(TagRequest _request)
        {
            if(_request.Id == 0)
            {
                BloggieSTag tags = new BloggieSTag()
                {
                    Name = _request.Name,
                    DisplayName = _request.DisplayName,
                    Active = true
                };
                _dbcontext.BloggieSTags.Add(tags);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("GetTagDetails");
            }
            else if(_request.Id != 0)
            {
                var existingTag = _dbcontext.BloggieSTags.FirstOrDefault(x=>x.Id == _request.Id);
                if (existingTag != null)
                {
                    existingTag.Name = _request.Name;
                    existingTag.DisplayName = _request.DisplayName;
                    existingTag.Active = true;
                    await _dbcontext.SaveChangesAsync();
                    return RedirectToAction("GetTagDetails");
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetTagDetails(long? TagId=0){
            var tags = await _service.GetTags(TagId);
            return View(tags);
        }
    }
}
