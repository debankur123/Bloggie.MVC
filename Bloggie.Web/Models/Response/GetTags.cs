namespace Bloggie.Web.Models.Response
{
    public class GetTags
    {
        public long TagId { get; set; }
        public string TagName { get; set; }
        public string DisplayName { get; set; }
        public Boolean Active { get; set; }
    }
}