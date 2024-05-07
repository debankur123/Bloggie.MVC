using Bloggie.Web.Models.Entity;
using Bloggie.Web.Models.Request;
using Bloggie.Web.Repository.DatabaseContext;
using Bloggie.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult> CreateTags(TagRequest request)
        {
            try
            {
                BloggieSTag tags = new BloggieSTag()
                {
                    Name = request.Name,
                    DisplayName = request.DisplayName,
                    Active = true
                };
                await _dbcontext.BloggieSTags.AddAsync(tags);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("GetTagDetails");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditTags(long TagId){
            var tag = await _dbcontext.BloggieSTags.FirstOrDefaultAsync(x=> x.Id == TagId && x.Active == true);
            try
            {
                if (tag == null) return View(null);
                var editTagRequest = new TagRequest
                {
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditTags(long TagId,TagRequest request){
            var existingTag = await _dbcontext.BloggieSTags.FirstOrDefaultAsync(x=>x.Id == TagId);
            try
            {
                if (existingTag != null)
                {
                    existingTag.Name = request.Name;
                    existingTag.DisplayName = request.DisplayName;
                    existingTag.Active = true;
                    await _dbcontext.SaveChangesAsync();
                    return RedirectToAction("GetTagDetails");
                }
                return RedirectToAction("EditTags", new { TagId = TagId });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetTagDetails(long? Id=0){
            var tags = await _service.GetTags(Id);
            return View(tags);
        }
        [HttpGet]
        public IActionResult DeleteTag(long TagId)
        {
            var tag = _dbcontext.BloggieSTags.FirstOrDefault(x => x.Id == TagId && x.Active == true);
            return PartialView("DeleteTag", tag);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTag(long TagId, TagRequest request) {
            var existingTag = await _dbcontext.BloggieSTags.FirstOrDefaultAsync(x => x.Id == TagId);
            try
            {
                if (existingTag != null)
                {
                    existingTag.Active = false;
                }
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("GetTagDetails");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
