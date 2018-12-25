using StudentsKingdom.Common.Constants.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsKingdom.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(UserConstants.UsernameMaxLength,MinimumLength = UserConstants.UsernameMinLength)]
        [RegularExpression(UserConstants.ValidUsernameRegex)]
        public string Username { get; set; }

        [Required]
        [StringLength(UserConstants.PasswordMaxLength, MinimumLength = UserConstants.PasswordMinLength)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
