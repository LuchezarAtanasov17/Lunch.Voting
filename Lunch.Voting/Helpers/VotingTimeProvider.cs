namespace Lunch.Voting.Helpers;

public class VotingTimeProvider(DateTimeOffset initialTime) 
    : TimeProvider
{
    private DateTimeOffset _currentTime = initialTime;

    public void SetTime(DateTimeOffset newTime) 
        => _currentTime = newTime;

    public override DateTimeOffset GetUtcNow() => _currentTime.UtcDateTime;
}
