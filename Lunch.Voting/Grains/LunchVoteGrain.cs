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
}
