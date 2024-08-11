using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.Auth;

public class Login
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
public class User
{
    public string Email        { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string UserName     { get; set; }
}

public class AuthenticationResult{
    public bool IsAuthenticated { get; set; }
    public string UserName      { get; set; }
    public string Role          { get; set; }
    public string Email         { get; set; }
    public string ErrorMessage  { get; set; }
}
