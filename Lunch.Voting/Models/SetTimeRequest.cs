namespace Lunch.Voting.Models;

public class SetTimeRequest
{
    public DateTime VoteDate { get; set; }

    public DateTimeOffset NewTime { get; set; }
}
