using System.Data;
using Bloggie.Web.Models.Entity;
using Bloggie.Web.Models.Request;
using Bloggie.Web.Models.Response;
using Bloggie.Web.Repository.DatabaseContext;
using Microsoft.Data.SqlClient;

namespace Bloggie.Web.Services
{
    public class BlogService
    {
        private readonly IConfiguration _configuration;
        private readonly BloggieWebContext _dbContext;
        public BlogService(IConfiguration configuration, BloggieWebContext webContext)
        {
            _configuration = configuration;
            _dbContext = webContext;
        }
        public async Task<dynamic> CreateTags(TagRequest request)
        {
            try
            {
                BloggieSTag tag = new BloggieSTag
                {
                    Name = request.Name,
                    DisplayName = request.DisplayName,
                    Active = true
                };
                _dbContext.BloggieSTags.Add(tag);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<GetTags>> GetTags(long? TagId=0)
        {
            SqlConnection con = new SqlConnection(Commonservice.getConnectionString());
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
                BloggieTBlogDtl obj = new BloggieTBlogDtl
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
                await _dbContext.BloggieTBlogDtls.AddAsync(obj);
                await _dbContext.SaveChangesAsync();
                if (request.SelectedTagIds != null)
                {
                    for (int idx = 0; idx < request.SelectedTagIds.Count(); idx++)
                    {
                        long TagIds = request.SelectedTagIds[idx];
                        BloggieMTag tags = new BloggieMTag()
                        {
                            BlogHdrid = obj.Id,
                            TagId     = TagIds,
                            Active    = true,
                        };
                        await _dbContext.BloggieMTags.AddAsync(tags);
                    }
                    await _dbContext.SaveChangesAsync();
                }
                return request;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
                        BlogId           = reader["BlogId"] != DBNull.Value ? (long)reader["BlogId"] : 0,
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
                        TagNames         = reader["TagNames"].ToString()?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
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
