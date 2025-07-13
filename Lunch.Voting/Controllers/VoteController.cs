using Lunch.Voting.Interfaces;
using Lunch.Voting.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lunch.Voting.Controllers;

[ApiController]
[Route("api/vote")]
public class VoteController(IGrainFactory grainFactory) 
    : ControllerBase
{
    private readonly IGrainFactory _grainFactory = grainFactory;

    [HttpPost("create")]
    public async Task<IActionResult> CreateVote(
        [FromQuery] 
        string user, 
        [FromBody] 
        CreateVoteRequest request)
    {
        if (string.IsNullOrEmpty(user))
        {
            return BadRequest("User parameter is required");
        }

        string dateKey = request.VoteDate.ToString("yyyy-MM-dd");
        var grain = _grainFactory.GetGrain<ILunchVoteGrain>(dateKey);

        var created = await grain.CreateVoteAsync(request.VoteDate);

        if (!created)
        {
            return Conflict("Vote already exists for this date");
        }

        return Ok(new { message = "Vote created successfully", voteDate = request.VoteDate });
    }

    [HttpPost("cast")]
    public async Task<IActionResult> CastVote(
        [FromQuery]
        string user, 
        [FromBody] 
        CastVoteRequest request)
    {
        if (string.IsNullOrEmpty(user))
        {
            return BadRequest("User parameter is required");
        }

        var dateKey = request.VoteDate.ToString("yyyy-MM-dd");
        var grain = _grainFactory.GetGrain<ILunchVoteGrain>(dateKey);

        var voted = await grain.CastVoteAsync(user, request.PlaceName);

        if (!voted)
        {
            return BadRequest("Unable to cast vote. Voting may be closed or you may have already voted.");
        }

        return Ok(new { message = "Vote cast successfully", user = user, placeName = request.PlaceName });
    }

    [HttpGet("results")]
    public async Task<IActionResult> GetResults(
        [FromQuery]
        string user, 
        [FromQuery] 
        DateTime date)
    {
        if (string.IsNullOrEmpty(user))
        {
            return BadRequest("User parameter is required");
        }

        var dateKey = date.ToString("yyyy-MM-dd");
        var grain = _grainFactory.GetGrain<ILunchVoteGrain>(dateKey);

        var results = await grain.GetResultsAsync();

        return Ok(results);
    }

    [HttpPost("set-time")]
    public async Task<IActionResult> SetTime(
        [FromQuery]
        string username, 
        [FromBody] 
        SetTimeRequest request)
    {
        if (username != "clock")
        {
            return Forbid("Only 'clock' user can set the time.");
        }

        var dateKey = request.VoteDate.ToString("yyyy-MM-dd");
        var grain = _grainFactory.GetGrain<ILunchVoteGrain>(dateKey);

        var success = await grain.SetServerTimeAsync(username, request.NewTime);

        return success 
            ? Ok("Time updated") 
            : StatusCode(500, "Failed to update time");
    }
}
