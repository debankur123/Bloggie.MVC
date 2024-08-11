using System;
using System.Collections.Generic;
using Bloggie.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BloggieMTag> BloggieMTags { get; set; }

    public virtual DbSet<BloggieSTag> BloggieSTags { get; set; }

    public virtual DbSet<BloggieTBlogDtl> BloggieTBlogDtls { get; set; }

    public virtual DbSet<BloggieTUploadedImg> BloggieTUploadedImgs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=BloggieWeb;User Id=SA;Password=Em@012173;MultipleActiveResultSets=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BloggieMTag>(entity =>
        {
            entity.ToTable("Bloggie_M_Tags");

            entity.Property(e => e.BlogHdrid).HasColumnName("BlogHDRId");
        });

        modelBuilder.Entity<BloggieSTag>(entity =>
        {
            entity.ToTable("Bloggie-S-Tags");

            entity.Property(e => e.DisplayName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BloggieTBlogDtl>(entity =>
        {
            entity.ToTable("Bloggie_T_BlogDTL");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Author)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BlogImageUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("BlogImageURL");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Heading)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.PageTitle)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.PublishedDate).HasColumnType("datetime");
            entity.Property(e => e.ShortDescription).HasMaxLength(500);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Urlhandle)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("URLHandle");
        });

        modelBuilder.Entity<BloggieTUploadedImg>(entity =>
        {
            entity.ToTable("Bloggie_T_UploadedImg");

            entity.Property(e => e.AppFileName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BlogHdrid).HasColumnName("BlogHDRId");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FileExt)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
