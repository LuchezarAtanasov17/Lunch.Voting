namespace Lunch.Voting.Models;

public class CastVoteRequest
{
    public DateTime VoteDate { get; set; }

    public string PlaceName { get; set; } = string.Empty;
}
