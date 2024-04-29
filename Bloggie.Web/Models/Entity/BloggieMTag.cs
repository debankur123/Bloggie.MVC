using System;
using System.Collections.Generic;

namespace Bloggie.Web.Models.Entity;

public partial class BloggieMTag
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public long? BlogHdrid { get; set; }

    public bool? Active { get; set; }
}
