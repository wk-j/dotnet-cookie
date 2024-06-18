using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDataProtection();

builder.Services
.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => {
    options.LoginPath = "/account/login";
    options.AccessDeniedPath = "/account/error";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set cookie life time instead of session
    options.Cookie.MaxAge = TimeSpan.FromMinutes(30); // Explicitly set the cookie lifetime
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

var cookiePolicyOptions = new CookiePolicyOptions {
    MinimumSameSitePolicy = SameSiteMode.Strict,
};

app.UseCookiePolicy(cookiePolicyOptions);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
