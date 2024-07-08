namespace Bloggie.Web.Models.Response
{
    public class BlogDetailsResponse
    {
        public long Id                 { get; set; }
        public string Heading          { get; set; }
        public string PageTitle        { get; set; }
        public string Content          { get; set; }
        public string ShortDescription { get; set; }
        public string BlogImageUrl     { get; set; }
        public string UrlHandle        { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string PublishedDateS   { get; set; }
        public string Author           { get; set; }
        public bool IsVisible          { get; set; }
        public DateTime? CreatedDate   { get; set; }
        public string CreatedDateS     { get; set; }
        public DateTime? UpdatedDate   { get; set; }
        public string UpdatedDateS     { get; set; }
        public int? UpdatedBy          { get; set; }
        public bool? Active            { get; set; }
        public string[] TagNames       { get; set; }
    }
}
