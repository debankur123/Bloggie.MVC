namespace Bloggie.Web.Models.Entity;

public partial class BloggieMTag
{
    public long Id { get; set; }

    public long? BlogHdrid { get; set; }

    public long? TagId { get; set; }

    public bool? Active { get; set; }
}
