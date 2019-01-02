using StudentsKingdom.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsKingdom.Common.Constants.User
{
    public static class UserConstants
    {
        public const string RolePlayer = "Player";
        public const string RoleAdmin = "Admin";

        public const int UsernameMinLength = 3;
        public const int UsernameMaxLength = 10;

        public const int PasswordMinLength = 5;
        public const int PasswordMaxLength = 15;

        public const string ValidUsernameAndPasswordRegex = @"[\w!$-]+";
    }
}
