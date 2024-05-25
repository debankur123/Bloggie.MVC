using System.Data;
using Bloggie.Web.Models.Entity;
using Bloggie.Web.Models.Request;
using Bloggie.Web.Models.Response;
using Bloggie.Web.Repository.DatabaseContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Services
{
    public class BlogService(IConfiguration configuration, BloggieWebContext webContext)
    {
        private readonly IConfiguration _configuration = configuration;
        private static readonly char[] Separator = new[] { ',' };

        public async Task<dynamic> CreateTags(TagRequest request)
        {
            try
            {
                var tag = new BloggieSTag
                {
                    Name = request.Name,
                    DisplayName = request.DisplayName,
                    Active = true
                };
                webContext.BloggieSTags.Add(tag);
                await webContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<GetTags>> GetTags(long? TagId=0)
        {
            var con = new SqlConnection(Commonservice.getConnectionString());
            await con.OpenAsync();
            try
            {
                SqlCommand cmd = new SqlCommand("USP_BLOGGIE_G_GetTags", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TagId", TagId);
                List<GetTags> tagsList = [];
                var dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                // tagsList.AddRange(from DataRow row in dt.Rows select new GetTags { TagId = (long)row["TagId"], TagName = row["TagName"].ToString(), DisplayName = row["DisplayName"].ToString(), Active = (bool)row["Active"] });
                // return tagsList;
                for (var index = 0; index < dt.Rows.Count; index++)
                {
                    var row = dt.Rows[index];
                    var result = new GetTags
                    {
                        TagId       = (long)row["TagId"],
                        TagName     = row["TagName"].ToString(),
                        DisplayName = row["DisplayName"].ToString(),
                        Active      = (bool)row["Active"]
                    };
                    tagsList.Add(result);
                }
                return tagsList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                await con.CloseAsync();
            }
        }

        public async Task<BlogPostRequest> PostBlogRequest(BlogPostRequest request)
        {
            try
            {
                var obj = new BloggieTBlogDtl
                {
                    Heading           = request.Heading,
                    PageTitle         = request.PageTitle,
                    Content           = request.Content,
                    ShortDescription  = request.ShortDescription,
                    BlogImageUrl      = request.BlogImageUrl,
                    Urlhandle         = request.UrlHandle,
                    PublishedDate     = request.PublishedDate,
                    Author            = request.Author,
                    IsVisible         = request.IsVisible,
                    Active            = true,
                    //CreatedBy = request.CreatedBy,
                    CreatedDate       = Commonservice.getIndianDatetime()
                };
                await webContext.BloggieTBlogDtls.AddAsync(obj);
                await webContext.SaveChangesAsync();
                if (request.SelectedTagIds == null) return request;
                for (var idx = 0; idx < request.SelectedTagIds.Length; idx++)
                {
                    var tagIds = request.SelectedTagIds[idx];
                    var tags = new BloggieMTag()
                    {
                        BlogHdrid = obj.Id,
                        TagId     = tagIds,
                        Active    = true,
                    };
                    await webContext.BloggieMTags.AddAsync(tags);
                }
                await webContext.SaveChangesAsync();
                return request;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<BlogPostRequest> EditBlogPost(BlogPostRequest request)
        {
            try
            {
                var blogToEdit = await webContext.BloggieTBlogDtls
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.Active == true);
                if (blogToEdit == null) return request;
                blogToEdit.Heading = request.Heading;
                blogToEdit.PageTitle = request.PageTitle;
                blogToEdit.Content = request.Content;
                blogToEdit.ShortDescription = request.ShortDescription;
                blogToEdit.BlogImageUrl = request.BlogImageUrl;
                blogToEdit.Urlhandle = request.UrlHandle;
                blogToEdit.PublishedDate = request.PublishedDate;
                blogToEdit.Author = request.Author;
                blogToEdit.IsVisible = request.IsVisible;
                blogToEdit.Active = true;
                blogToEdit.UpdatedDate = Commonservice.getIndianDatetime();
                await webContext.SaveChangesAsync();
                var existingTagOfBlog = await webContext.BloggieMTags
                    .Where(a => a.BlogHdrid == blogToEdit.Id && a.Active==true).ToListAsync();
                foreach (var item in existingTagOfBlog)
                {
                    item.Active = false;
                }
                foreach (var tagIds in request.SelectedTagIds)
                {
                    var tags = new BloggieMTag
                    {
                        BlogHdrid = blogToEdit.Id,
                        TagId = tagIds,
                        Active = true
                    };
                    await webContext.BloggieMTags.AddAsync(tags);
                }
                await webContext.SaveChangesAsync();
                return request;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while processing the request" + e.Message);
            }
        }
        public static async Task<IEnumerable<BlogDetailsResponse>> GetBlogDetails(long? BlogId=0)
        {
            var connection = new SqlConnection(Commonservice.getConnectionString());
            await connection.OpenAsync();
            try
            {
                var cmd = new SqlCommand("USP_Blogging_G_BlogDetails",connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BlogId", BlogId);
                var reader = await cmd.ExecuteReaderAsync();
                var blogDetailsList = new List<BlogDetailsResponse>();
                if (!reader.HasRows) return blogDetailsList;
                while (await reader.ReadAsync())
                {
                    var blogDetailsResponse = new BlogDetailsResponse
                    {
                        Id               = reader["Id"] != DBNull.Value ? (long)reader["Id"] : 0,
                        Heading          = reader["Heading"].ToString(),
                        PageTitle        = reader["PageTitle"].ToString(),
                        Content          = reader["Content"].ToString(),
                        ShortDescription = reader["ShortDescription"].ToString(),
                        BlogImageUrl     = reader["BlogImageUrl"].ToString(),
                        UrlHandle        = reader["UrlHandle"].ToString(),
                        PublishedDate    = (DateTime)reader["PublishedDate"],
                        PublishedDateS   = reader["PublishedDateS"].ToString(),
                        Author           = reader["Author"].ToString(),
                        IsVisible        = reader["IsVisible"] != DBNull.Value && (bool)reader["IsVisible"],
                        CreatedDateS     = reader["CreatedDateS"].ToString(),
                        UpdatedDateS     = reader["UpdatedDateS"].ToString(),
                        TagNames         = reader["TagNames"].ToString()?.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                    };
                    blogDetailsList.Add(blogDetailsResponse);
                }
                return blogDetailsList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { await connection.CloseAsync(); }
        }
    }
}
