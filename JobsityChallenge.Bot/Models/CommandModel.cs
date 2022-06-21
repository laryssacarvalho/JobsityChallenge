namespace JobsityChallenge.Bot.Models;

public class CommandModel
{
    public string Command { get; set; }
    public string Value { get; set; }
    public int ChatId { get; set; }
    public bool IsValid() => Constants.ValidCommands.Contains(Command);
}
