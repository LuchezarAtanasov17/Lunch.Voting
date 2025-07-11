namespace Lunch.Voting.Models;

public class Vote
{
    public DateTime Date { get; set; }

    public string PlaceName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public DateTime VotedAt { get; set; }
}
