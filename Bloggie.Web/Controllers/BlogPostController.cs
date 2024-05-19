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
            var postModel = await service.PostBlogRequest(request);
            return RedirectToAction("CreateBlogPost");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
            BlogId = blogDetail?.BlogId ?? 0,
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
                Text = tag.DisplayName
            }),
            SelectedTagIds = blogDetail?.TagNames?.Select(t => tags.FirstOrDefault
                (tag => tag.DisplayName == t)?.TagId ?? 0).ToArray()
        };
        return View(viewModel);
    }
}