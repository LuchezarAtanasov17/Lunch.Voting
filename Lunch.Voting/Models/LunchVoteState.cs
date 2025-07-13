namespace Lunch.Voting.Models;

public class LunchVoteState(TimeProvider timeProvider)
{
    private static readonly TimeSpan RESULTS_END_TIME = TimeSpan.FromHours(13.5);
    private TimeProvider _timeProvider = timeProvider;

    public DateTime VoteDate { get; set; }

    public TimeSpan VoteEndTime { get; set; } = TimeSpan.FromHours(11.5);

    public List<Vote> Votes { get; set; } = new();

    public List<string> LunchPlaces { get; set; } = new()
    {
        "Pizza Palace",
        "Burger Bros",
        "Sushi Station",
        "Taco Town",
        "Salad Spot",
    };

    public bool IsVotingClosed => _timeProvider.GetUtcNow().ToLocalTime().TimeOfDay > VoteEndTime;

    public bool CanShowResults => IsVotingClosed && _timeProvider.GetUtcNow().ToLocalTime().TimeOfDay <= RESULTS_END_TIME;
}
