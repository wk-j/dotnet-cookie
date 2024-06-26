using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyWeb.Controllers.Hello;

public class LoginInfo {
    public string Username { set; get; }
    public string Password { set; get; }
}

[Route("account")]
public class AccountController(ILogger<AccountController> logger) : Controller {
    [HttpGet("login")]
    public async Task<ActionResult> Login(string returnUrl = "/account/index") {
        if (User.Identity?.IsAuthenticated == true) {
            logger.LogInformation("User is already authenticated");
            return Redirect(returnUrl);
        }
        return View();
    }

    [HttpGet("logout")]
    public async Task<ActionResult> Logout() {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties {
            ExpiresUtc = DateTimeOffset.UtcNow,
            IsPersistent = false
        });
        return Redirect("/account/login");
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login2([FromForm] LoginInfo info, string returnUrl = "/account/index") {
        if (info.Password == "11") {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, info.Username),
                new Claim(ClaimTypes.Country, "Thailand"),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Redirect(returnUrl);
        } else {
            return Redirect("login");
        }
    }

    [Authorize]
    [HttpGet("get-name")]
    public string GetName() {
        var nameClaim = User.FindFirst(ClaimTypes.Name)?.Value;
        return nameClaim ?? "No name found";
    }

    [HttpGet("error")]
    public string Error() {

        return "Please login";
    }

    [HttpGet("index")]
    [Authorize]
    public ActionResult Index() {
        return View();
    }
}
