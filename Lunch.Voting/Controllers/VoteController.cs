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
    public async Task<IActionResult> CreateVote([FromQuery] string user, [FromBody] CreateVoteRequest request)
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
}
