using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.DataProtection;

namespace MyWeb.Controllers.My;

[Route("api/[controller]")]
[ApiController]
public class MyController : ControllerBase {
    private readonly IDataProtectionProvider _dataProtectionProvider;

    public MyController(IDataProtectionProvider dataProtectionProvider) {
        _dataProtectionProvider = dataProtectionProvider;
    }

    [HttpGet("get-cookie")]
    public ActionResult<string> GetCookie() {
        // Read the cookie from the request
        var protectedText = HttpContext.Request.Cookies["MyCookie"];
        if (protectedText != null) {
            // Decrypt the cookie
            var protector = _dataProtectionProvider.CreateProtector("Your-purpose-string");
            var plainText = protector.Unprotect(protectedText);
            return plainText;
        }
        return string.Empty;
    }

    [HttpGet("create-cookie")]
    public IActionResult CreateCookie() {
        // Encrypt the cookie
        var plainText = "my cookie value";
        var protector = _dataProtectionProvider.CreateProtector("Your-purpose-string");
        var protectedText = protector.Protect(plainText);
        // Add the encrypted cookie to the response
        HttpContext.Response.Cookies.Append("MyCookie", protectedText);
        return Ok();
    }
}
