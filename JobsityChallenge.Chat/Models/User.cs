using Microsoft.AspNetCore.Identity;

namespace JobsityChallenge.Chat.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
