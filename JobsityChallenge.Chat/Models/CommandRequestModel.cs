namespace JobsityChallenge.Chat.Models
{
    public class CommandRequestModel
    {
        public string Command { get; private set; }
        public string Value { get; private set; }
        public CommandRequestModel(string command, string value)
        {
            Command = command;
            Value = value;
        }
    }
}
