using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsKingdom.Common.Constants
{
    public static class ExceptionMessages
    {
        public const string ViewDataErrorKey = "Error";

        public const string LoginError = "Invalid Username or Password!";
        public const string RegisterError = "Username or Email are already taken!";
        public const string InvalidUsernameOrPasswordRegex = "Uncommon symbols are restricted!";

        public const string FullInventory = "Your inventory is full!";
        public const string ChooseItem = "You should choose an item in order to buy!";
        public const string CannotAfford = "You do not have enough coins!";
        public const string ItemAlreadyBought = "You already have this item in your inventory!";
        
    }
}
