using Microsoft.AspNetCore.Identity;

namespace JobsityChallenge.Chat.Entities;

public class UserEntity : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<MessageEntity> Messages { get; set; }
}
