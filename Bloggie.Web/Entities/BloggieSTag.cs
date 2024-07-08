using System;
using System.Collections.Generic;

namespace Bloggie.Web.Entities;

public partial class BloggieSTag
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public bool? Active { get; set; }
}
