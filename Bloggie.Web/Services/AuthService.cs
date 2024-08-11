using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bloggie.Web.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace Bloggie.Web.Services
{
    public class AuthService
    {
        private readonly string _connectionString;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public AuthService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("dbConnection");
            _jwtKey = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
        }

        public async Task RegisterUserAsync(Register model)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("USP_Ins_RegisterAdminOrUser", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@UserId",   model.Email);
            command.Parameters.AddWithValue("@Password", model.Password);
            command.Parameters.AddWithValue("@UserName", model.UserName);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<AuthenticationResult> AuthenticateUserAsync(Login _loginParams)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("USP_G_Login", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Email", _loginParams.Email);
            command.Parameters.AddWithValue("@Password", _loginParams.Password);

            var isAuthenticatedParam = new SqlParameter("@IsAuthenticated", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(isAuthenticatedParam);

            var userNameParam = new SqlParameter("@UserName", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(userNameParam);

            var roleParam = new SqlParameter("@Role", SqlDbType.NVarChar, 50)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(roleParam);

            var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, -1)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(errorMessageParam);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            var isAuthenticated = (bool)isAuthenticatedParam.Value;
            var userName = userNameParam.Value as string;
            var role = roleParam.Value as string;
            var errorMessage = errorMessageParam.Value as string;

            return new AuthenticationResult
            {
                IsAuthenticated = isAuthenticated,
                UserName = userName,
                Role = role,
                Email = _loginParams.Email,
                ErrorMessage = errorMessage
            };
        }
        public string GenerateJwtToken(string email, string role, string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("UserName", userName)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT Email, PasswordHash, PasswordSalt, UserName FROM Users WHERE Email = @Email", connection);
            command.Parameters.AddWithValue("@Email", email);
            User user = null;
            try
            {
                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    user = new User
                    {
                        Email = reader["Email"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString(),
                        PasswordSalt = reader["PasswordSalt"].ToString(),
                        UserName = reader["UserName"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

    }
}
