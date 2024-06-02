using Bloggie.Web.Models.Request;
using Bloggie.Web.Models.Response;
using Bloggie.Web.Repository.DatabaseContext;
using Bloggie.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers;
public class BlogPostController(BlogService service, BloggieWebContext context) : Controller
{
    [HttpGet]
    public async Task<ActionResult> BlogDetails(long? BlogId = 0) {
        var blogDetails = await BlogService.GetBlogDetails(BlogId);
        return View((List<BlogDetailsResponse>)blogDetails);
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
        try
        {
            await service.PostBlogRequest(request);
            return RedirectToAction("BlogDetails");
        }
        catch (Exception)
        {
            return RedirectToAction("CreateBlogPost");
        }
    }
    [HttpGet]
    public async Task<ActionResult> EditBlogPost(long? BlogId = 0)
    {
        var blogDetails = await BlogService.GetBlogDetails(BlogId);
        var blogDetail =  blogDetails.FirstOrDefault();
        var tags = await service.GetTags();
        var viewModel = new BlogPostRequest
        {
            Id = blogDetail?.Id ?? 0,
            Heading = blogDetail?.Heading ?? string.Empty,
            PageTitle = blogDetail?.PageTitle ?? string.Empty,
            Content = blogDetail?.Content ?? string.Empty,
            ShortDescription = blogDetail?.ShortDescription ?? string.Empty,
            BlogImageUrl = blogDetail?.BlogImageUrl ?? string.Empty,
            UrlHandle = blogDetail?.UrlHandle ?? string.Empty,
            PublishedDate = blogDetail?.PublishedDate,
            PublishedDateS = blogDetail?.PublishedDateS,
            Author = blogDetail?.Author ?? string.Empty,
            IsVisible = blogDetail?.IsVisible ?? false,
            CreatedDate = blogDetail?.CreatedDate ?? (DateTime?)null,
            UpdatedDate = blogDetail?.UpdatedDate ?? (DateTime?)null,
            Active = blogDetail?.Active ?? false,
            Tags = tags.Select(tag => new SelectListItem
                {
                    Value = tag.TagId.ToString(),
                    Text = tag.TagName
                }
            ),
            SelectedTagIds = blogDetail?.TagNames?.Select(t => tags.FirstOrDefault
                (tag => tag.TagName == t)?.TagId ?? 0).ToArray()
        };
        return View(viewModel);
    }
    [HttpPost]
    public async Task<ActionResult> EditBlogPost(BlogPostRequest request)
    {
        try
        {
            if (request.Id == 0)
            {
                throw new Exception("Invalid Request");
            }
            await service.EditBlogPost(request);
            TempData["SuccessMessage"] = "Blog post edited successfully.";
            return RedirectToAction("BlogDetails", new { id = request.Id });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            return RedirectToAction("EditBlogPost", new { id = request.Id });
        }
    }
    [HttpGet]
    public ActionResult DeleteBlogPost(long Id)
    {
        var blog = context.BloggieTBlogDtls.FirstOrDefault(x => x.Id == Id && x.Active == true);
        return PartialView("DeleteBlogPost", blog);
    }

    [HttpPost,ActionName("DeleteBlogPost")]
    public async Task<ActionResult> DeleteBlogPost(long blogId,BlogPostRequest request)
    {
        var response = await service.DeleteBlogPost(blogId);
        if (response.StatusCode == "1")
        {
            return RedirectToAction("BlogDetails");
        }
        ModelState.AddModelError("", response.StatusMessage);
        var blog = context.BloggieTBlogDtls.FirstOrDefault(x => x.Id == blogId && x.Active == true);
        return PartialView("DeleteBlogPost", blog);
    }
}