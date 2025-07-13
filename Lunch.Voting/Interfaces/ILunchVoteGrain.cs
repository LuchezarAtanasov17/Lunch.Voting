using Lunch.Voting.Models;

namespace Lunch.Voting.Interfaces;

public interface ILunchVoteGrain : IGrainWithStringKey
{
    public Task<bool> CreateVoteAsync(string userName, DateTime voteDate);

    public Task<bool> CastVoteAsync(string userName, string placeName);

    public Task<List<VoteResult>> GetResultsAsync();

    public Task<bool> SetServerTimeAsync(string userName, DateTimeOffset newTime);

}
