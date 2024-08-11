using Bloggie.Web.Models.Auth;
using Bloggie.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AuthController(AuthService authService) : Controller
    {
        private readonly AuthService _authService = authService;

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                await _authService.RegisterUserAsync(model);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var authResult = await _authService.AuthenticateUserAsync(model);
                if (authResult.IsAuthenticated)
                {
                    var token = _authService.GenerateJwtToken(authResult.Email, authResult.Role, authResult.UserName);
                    HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        HttpOnly = true, // Makes the cookie inaccessible to JavaScript
                        Secure = true, // Only sends the cookie over HTTPS
                        SameSite = SameSiteMode.Lax // Helps prevent CSRF attacks
                    });
                    HttpContext.Response.Cookies.Append("UserRole", authResult.Role, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax
                    });
                    HttpContext.Response.Cookies.Append("UserName", authResult.UserName, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax
                    });
                    HttpContext.Response.Cookies.Append("UserEmail", model.Email, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax
                    });

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Logout(){
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("UserName");
            Response.Cookies.Delete("UserRole");
            Response.Cookies.Delete("UserEmail");
            return RedirectToAction("Index","Home");
        }

    }
}
