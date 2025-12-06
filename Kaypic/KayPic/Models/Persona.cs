using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;

namespace KayPic.Models
{
    public class Persona : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public Location? Location { get; set; }



    }

}
