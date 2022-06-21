namespace JobsityChallenge.Chat.Models;

public class CommandRequestModel
{
    public string Command { get; private set; }
    public string Value { get; private set; }
    public int ChatId { get; set; }
    public CommandRequestModel(string command, string value, int chatId)
    {
        Command = command;
        Value = value;
        ChatId = chatId;
    }
}
