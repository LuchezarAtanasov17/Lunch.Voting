namespace Lunch.Voting.Models;

[GenerateSerializer]
public class VoteResult
{
    public string PlaceName { get; set; } = string.Empty;

    public int Count { get; set; }
}
