using Microsoft.AspNetCore.Identity;
using System;

namespace StudentsKingdom.Data.Models
{
    public class Player : IdentityUser
    {
        public int? CharacterId { get; set; }
        public virtual Character Character { get; set; }
    }
}
