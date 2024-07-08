using System;
using System.Collections.Generic;

namespace Bloggie.Web.Entities;

public partial class BloggieTBlogDtl
{
    public long Id { get; set; }

    public string Heading { get; set; }

    public string PageTitle { get; set; }

    public string Content { get; set; }

    public string ShortDescription { get; set; }

    public string BlogImageUrl { get; set; }

    public string Urlhandle { get; set; }

    public DateTime? PublishedDate { get; set; }

    public string Author { get; set; }

    public bool? IsVisible { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public bool? Active { get; set; }
}
