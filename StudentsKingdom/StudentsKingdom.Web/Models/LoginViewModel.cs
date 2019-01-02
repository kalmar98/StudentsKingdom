using StudentsKingdom.Common.Constants;
using StudentsKingdom.Common.Constants.User;
using System.ComponentModel.DataAnnotations;

namespace StudentsKingdom.Web.Models
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(UserConstants.ValidUsernameAndPasswordRegex, ErrorMessage = ExceptionMessages.InvalidUsernameOrPasswordRegex)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(UserConstants.ValidUsernameAndPasswordRegex, ErrorMessage = ExceptionMessages.InvalidUsernameOrPasswordRegex)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
