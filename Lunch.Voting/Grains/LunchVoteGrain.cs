using Lunch.Voting.Interfaces;
using Lunch.Voting.Models;

namespace Lunch.Voting.Grains;

public class LunchVoteGrain : Grain, ILunchVoteGrain
{
    private LunchVoteState _state = new();

    public Task<bool> CreateVoteAsync(DateTime voteDate)
    {
        if (_state.VoteDate.Date == voteDate.Date)
        {
            return Task.FromResult(false);
        }

        _state.VoteDate = voteDate.Date;
        _state.VoteEndTime = voteDate.AddMinutes(5);

        return Task.FromResult(true);
    }

    public Task<bool> CastVoteAsync(string userName, string placeName)
    {
        if (_state.IsVotingClosed)
        {
            return Task.FromResult(false);
        }
        if (_state.Votes.Any(x => x.UserName == userName))
        {
            return Task.FromResult(false);
        }
        if (!_state.LunchPlaces.Contains(placeName))
        {
            return Task.FromResult(false);
        }

        var vote = new Vote
        {
            Date = _state.VoteDate,
            PlaceName = placeName,
            UserName = userName,
            VotedAt = DateTime.UtcNow,
        };

        _state.Votes.Add(vote);

        return Task.FromResult(true);
    }
}
