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
                        TagId = (long)row["TagId"],
                        TagName = row["TagName"].ToString(),
                        DisplayName = row["DisplayName"].ToString(),
                        Active = (bool)row["Active"]
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
    }
}
