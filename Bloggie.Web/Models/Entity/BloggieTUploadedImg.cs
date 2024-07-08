using System;
using System.Collections.Generic;

namespace Bloggie.Web.Models.Entity;

public partial class BloggieTUploadedImg
{
    public long Id { get; set; }

    public long? BlogHdrid { get; set; }

    public int? FileRowIndex { get; set; }

    public string FileName { get; set; }

    public string AppFileName { get; set; }

    public string FileExt { get; set; }

    public string FilePath { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? Active { get; set; }
}
