using APBD12.DTOs;
using APBD12.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTripPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Błędne dane wejściowe");
        }

        try
        {
            var answear = await _tripService.GetTripPageAsync(page, pageSize);
            return Ok(answear);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}