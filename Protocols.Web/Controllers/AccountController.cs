using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    [HttpGet]
    [Route("login")]
    public IActionResult Login() => View();

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        // Примітивна перевірка логіна
        if (username == "admin" && password == "Q!W@E#r4t5y6")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);

            return RedirectToAction("Index", "Protocols"); // або інша сторінка
        }

        ViewBag.Error = "Invalid credentials";
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction("Login");
    }
}