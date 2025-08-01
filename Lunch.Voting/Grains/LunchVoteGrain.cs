﻿using Lunch.Voting.Helpers;
using Lunch.Voting.Interfaces;
using Lunch.Voting.Models;

namespace Lunch.Voting.Grains;

public class LunchVoteGrain(TimeProvider timeProvider)
    : Grain, ILunchVoteGrain
{
    private readonly TimeProvider _timeProvider = timeProvider;
    private LunchVoteState _state = new LunchVoteState(timeProvider);

    public Task<bool> CreateVoteAsync(string userName, DateTime voteDate)
    {
        if (userName == "clock")
        {
            return Task.FromResult(false);
        }
        if (_state.VoteDate.Date == voteDate.Date)
        {
            return Task.FromResult(false);
        }
        if (_state.VoteEndTime < voteDate.Date.TimeOfDay)
        {
            return Task.FromResult(false);
        }

        _state.VoteDate = voteDate.Date;

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
            VotedAt = _timeProvider.GetUtcNow().ToLocalTime().Date,
        };

        _state.Votes.Add(vote);

        return Task.FromResult(true);
    }

    public Task<List<VoteResult>> GetResultsAsync()
    {
        if (!_state.CanShowResults)
        {
            return Task.FromResult(new List<VoteResult>());
        }

        List<VoteResult> results = _state.Votes
            .GroupBy(x => x.PlaceName)
            .Select(x => new VoteResult
            {
                PlaceName = x.Key,
                Count = x.Count()
            })
            .ToList();

        foreach (var place in _state.LunchPlaces)
        {
            if (!results.Any(x => x.PlaceName == place))
            {
                var voteResult = new VoteResult
                {
                    PlaceName = place,
                    Count = 0,
                };

                results.Add(voteResult);
            }
        }

        List<VoteResult> sortedResults = results
            .OrderByDescending(x => x.Count)
            .ToList();

        return Task.FromResult(sortedResults);
    }

    public Task<bool> SetServerTimeAsync(string userName, DateTimeOffset newTime)
    {
        if (userName != "clock")
        {
            return Task.FromResult(false);
        }

        if (_timeProvider is VotingTimeProvider votingTimeProvider)
        {
            votingTimeProvider.SetTime(newTime);

            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}
