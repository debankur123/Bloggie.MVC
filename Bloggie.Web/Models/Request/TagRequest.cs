namespace Bloggie.Web.Models.Request
{
    public class TagRequest
    {
        public long Id            { get; set; }
        public string Name        { get; set; }
        public string DisplayName { get; set; }
        public bool Active        { get; set; }
    }
}
