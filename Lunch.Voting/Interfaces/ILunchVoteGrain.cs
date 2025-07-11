namespace Lunch.Voting.Interfaces;

public interface ILunchVoteGrain : IGrainWithStringKey
{
    public Task<bool> CreateVoteAsync(DateTime voteDate);
}
