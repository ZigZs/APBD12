using APBD12.DTOs;
using APBD12.Exceptions;
using APBD12.Models;
using APBD12.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD12.Controllers;


[ApiController]
[Route("api")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpDelete("clients/{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        try
        {
            await _clientService.DeleteClientAsync(id);
            return Ok("Klient deleted");
        }
        catch (ClientNotFoundException ex)
        {
            return NotFound(new {messege = ex.Message});
        }
        catch (ClientAssignTripException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("trips/{tripId}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip,[FromBody] AddClientToTripDTO addClientToTripDto)
    {
        try
        {
            await _clientService.AddClientToTripAsync(idTrip, addClientToTripDto);
            return Ok("Dodano klienta do wycieczki");
        }
        catch (ClientNotFoundException ex)
        {
            return NotFound(new { messege = ex.Message });
        }
        catch (TripNotFoundException ex)
        {
            return NotFound(new { messege = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}