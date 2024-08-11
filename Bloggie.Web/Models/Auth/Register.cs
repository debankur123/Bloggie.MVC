using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.Auth
{
    public class Register
    {
        //[Required]
        //[StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }

        //[Required]
        //[EmailAddress]
        public string Email { get; set; }

        //[Required]
        //[StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
    public class RegisterResult{
        public bool Success   { get; set; }
        public string Message { get; set; }
    }
}