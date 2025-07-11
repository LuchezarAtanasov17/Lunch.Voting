namespace Lunch.Voting.Models;

public class LunchVoteState
{
    public DateTime VoteDate { get; set; }

    public DateTime VoteEndTime { get; set; }

    public List<Vote> Votes { get; set; } = new();

    public List<string> LunchPlaces { get; set; } = new()
    {
        "Pizza Palace",
        "Burger Bros",
        "Sushi Station",
        "Taco Town",
        "Salad Spot",
    };

    public bool IsVotingClosed => DateTime.UtcNow > VoteEndTime;

    public bool CanShowResults => IsVotingClosed && DateTime.UtcNow <= VoteEndTime.AddMinutes(15);
}
