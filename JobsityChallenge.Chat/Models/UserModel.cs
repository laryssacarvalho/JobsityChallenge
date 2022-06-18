using Microsoft.AspNetCore.Identity;

namespace JobsityChallenge.Chat.Models
{
    public class UserModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<MessageModel> Messages { get; set; }
    }
}
