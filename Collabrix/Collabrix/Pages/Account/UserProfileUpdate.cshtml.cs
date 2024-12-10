using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace Collabrix.Pages.Account
{
    public class UserProfileUpdateModel : PageModel
    {
        [BindProperty, Required(ErrorMessage = "Full Name is required.")]
        public string? name { get; set; }

        [BindProperty, Required, EmailAddress(ErrorMessage = "A valid email address is required.")]
        public string? mail { get; set; }

        [BindProperty, Required(ErrorMessage = "Current password is required.")]
        public string? Pass { get; set; }

        [BindProperty, Required(ErrorMessage = "New password is required."), MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? Password { get; set; }

        [BindProperty, Required(ErrorMessage = "Confirming new password is required.")]
        public string? ConfirmPassword { get; set; }


        // Hidden Attributes
        [BindProperty]
        public int? useId { get; set; }
        
        [BindProperty]
        public string? CheckPass { get; set; }


        // Private attributes
        private User? privateUser = new User();


        public async Task OnGetAsync()
        {
            var userId = User.FindFirst("uId")?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int uId))
            {
                TempData["ErrorOnServer"] = "Invalid user ID.";
                return;
            }

            User us = await UserController.GetUser(uId);
            if (us == null)
            {
                TempData["ErrorOnServer"] = "User not found.";
            }
            useId = us.UserId;
            name = us.FullName;
            mail = us.Email;
            CheckPass = us.PasswordHash;
        }

        public async Task<IActionResult> OnPostUpdateUserDetailsAsync()
        {
            try
            {
                privateUser.UserId = useId ?? 0;
                privateUser.FullName = name ?? string.Empty;
                privateUser.Email = mail ?? string.Empty;
                privateUser.PasswordHash = CheckPass ?? string.Empty;
                if (await UserController.UpdateUser(privateUser))
                {
                    TempData["Success"] = "User details updated successfully.";
                    return RedirectToPage("/Account/UserProfileUpdate");
                }
                TempData["ErrorOnServer"] = "Failed to update user details.";
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = $"An error occurred: {ex.Message}";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateUserPasswordAsync()
        {
            if (string.IsNullOrEmpty(Pass) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
            {
                TempData["ErrorOnServer"] = "Invalid input data.";
                return Page();
            }
            if (Pass != CheckPass)
            {
                TempData["ErrorOnServer"] = "Invalid password.";
                return Page();
            }
            if (Password != ConfirmPassword)
            {
                TempData["ErrorOnServer"] = "Passwords do not match.";
                return Page();
            }
            try
            {
                privateUser.UserId = useId ?? 0;
                privateUser.FullName = name ?? string.Empty;
                privateUser.Email = mail ?? string.Empty;
                privateUser.PasswordHash = Password;
                if (await UserController.UpdateUser(privateUser))
                {
                    TempData["Success"] = "Password updated successfully.";
                    return RedirectToPage("/Account/UserProfileUpdate");
                }
                TempData["ErrorOnServer"] = "Failed to update password.";
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = $"An error occurred: {ex.Message}";
            }
            return Page();
        }
    }
}
