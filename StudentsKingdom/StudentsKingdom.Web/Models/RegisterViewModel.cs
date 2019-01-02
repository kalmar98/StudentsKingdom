using StudentsKingdom.Common.Constants;
using StudentsKingdom.Common.Constants.User;
using System.ComponentModel.DataAnnotations;

namespace StudentsKingdom.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(UserConstants.UsernameMaxLength,MinimumLength = UserConstants.UsernameMinLength)]
        [RegularExpression(UserConstants.ValidUsernameAndPasswordRegex, ErrorMessage = ExceptionMessages.InvalidUsernameOrPasswordRegex)]
        public string Username { get; set; }

        [Required]
        [StringLength(UserConstants.PasswordMaxLength, MinimumLength = UserConstants.PasswordMinLength)]
        [RegularExpression(UserConstants.ValidUsernameAndPasswordRegex, ErrorMessage = ExceptionMessages.InvalidUsernameOrPasswordRegex)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
