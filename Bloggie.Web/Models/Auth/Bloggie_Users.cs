namespace Bloggie.Web.Models.Auth;

public class Bloggie_Users
{
    public int     UserId       { get; set; }
    public string? Username     { get; set; }
    public string? PasswordHash { get; set; }
    public int     RoleId       { get; set; }
    public bool    Active       { get; set; }
}