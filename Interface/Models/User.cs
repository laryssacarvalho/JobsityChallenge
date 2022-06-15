using Microsoft.AspNetCore.Identity;

namespace Interface.Models
{
    public class User : IdentityUser
    {
        public List<Message> Messages { get; set; }
    }
}
