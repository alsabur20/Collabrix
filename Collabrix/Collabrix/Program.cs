using Collabrix.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");  //be sure add this line


builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Initialize the controllers
UserController.Initialize(builder.Configuration);
TaskController.Initialize(builder.Configuration);
ProjectTaskStageController.Initialize(builder.Configuration);
ProjectController.Initialize(builder.Configuration);
LookUpcontroller.Initialize(builder.Configuration);
UserProjectController.Initialize(builder.Configuration);
NotificationsController.Initialize(builder.Configuration);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToPage("/Account/UserSignIn");
    options.Conventions.AllowAnonymousToPage("/Account/UserSignUp");
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/UserSignIn";
                options.LogoutPath = "/Account/UserSignOut";
            })
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = builder.Configuration.GetSection("Authentication:Google:ClientId").Value;
                options.ClientSecret = builder.Configuration.GetSection("Authentication:Google:ClientSecret").Value;
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session middleware
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
