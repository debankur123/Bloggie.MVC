using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Models.Request;

public class BlogPostRequest
{
    public long Id                 { get; set; }
    public string Heading          { get; init; }
    public string PageTitle        { get; init; }
    public string Content          { get; init; }
    public string ShortDescription { get; init; }
    public string BlogImageUrl     { get; init; }
    public string UrlHandle        { get; init; }
    public DateTime? PublishedDate { get; init; }
    public string PublishedDateS   { get; set; }
    public string Author           { get; init; }
    public bool IsVisible          { get; init; }
    public DateTime? CreatedDate   { get; init; }
    public int? CreatedBy          { get; init; }
    public DateTime? UpdatedDate   { get; init; }
    public int? UpdatedBy          { get; init; }
    public bool? Active            { get; init; }
    public IEnumerable<SelectListItem> Tags { get; init; }
    public long[] SelectedTagIds { get; init; } = [];
}