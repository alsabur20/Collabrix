using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using Collabrix.Models;
using Microsoft.AspNetCore.Authorization;
using Collabrix.Controllers;
using System.Data;
using System.Net;
using Collabrix.Helper;
using Microsoft.AspNetCore.Authentication.Google;


namespace Collabrix.Pages.Account
{
    [AllowAnonymous]
    public class SignInModel : PageModel
    {
        [BindProperty]
        public string? Email { get; set; }
        [BindProperty]
        public string? Password { get; set; }
        [BindProperty]
        private User User { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnGetLogin()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.Page("/Account/UserSignIn", "GoogleResponse")
            }, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> OnGetGoogleResponse()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (!result.Succeeded || result.Principal == null)
                {
                    TempData["ErrorOnServer"] = "Google authentication failed.";
                    return RedirectToPage("/Account/SignIn");
                }

                // Extract specific claims from the Google response
                var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
                var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    TempData["ErrorOnServer"] = "Google account does not have an associated email.";
                    return RedirectToPage("/Account/SignIn");
                }

                // Check if the user exists in the database
                User = await UserController.GetUser(email);
                if (User == null)
                {
                    User newUser = new User
                    {
                        Email = email,
                        FullName = name,
                        PasswordHash = "",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false

                    };
                    int newUserId = await UserController.AddUser(newUser);
                    User = await UserController.GetUser(newUserId);
                }

                // Sign in the user
                var claims = new List<Claim>
        {
            new Claim("uId", User.UserId.ToString()),
            new Claim(ClaimTypes.Name, User.FullName ?? ""),
            new Claim(ClaimTypes.Email, User.Email ?? "")
        };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
                return RedirectToPage("/Account/SignIn");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                bool isValid = await ValidateUser(Email, Password);
                if (isValid)
                {
                    var claims = new List<Claim>
                        {
                            new Claim("uId", User.UserId.ToString()),
                            new Claim(ClaimTypes.Name, User.FullName.ToString()),
                            new Claim(ClaimTypes.Email, User.Email.ToString()),
                        };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToPage("/Index");
                }
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] += ex.Message;

                return Page();
            }
        }

        private async Task<bool> ValidateUser(string username, string password)
        {
            try
            {
                string passwordHash = password;
                User = await UserController.GetUser(username, passwordHash);
                if (User != null && !User.IsDeleted)
                {
                    //bool isValid = PasswordHasher.VerifyPassword(password, User.PasswordHash);
                    bool isValid = true;
                    if (isValid)
                    {
                        return true;
                    }
                    else
                    {
                        TempData["ErrorOnServer"] = "Invalid Credentials..!!";
                        return false;
                    }
                }
                else
                {
                    TempData["ErrorOnServer"] = "User not found";
                    return false;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
                return false;
            }
        }
    }
}