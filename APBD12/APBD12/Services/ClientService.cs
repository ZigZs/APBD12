using APBD12.Data;
using APBD12.DTOs;
using APBD12.Exceptions;
using APBD12.Models;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace APBD12.Services;

public class ClientService : IClientService
{
    private readonly Apbd12Context _context;

    public ClientService(Apbd12Context context)
    {
        _context = context;
    }

    public async Task DeleteClientAsync(int idClient)
    {
        var client = await _context.Clients
            .Include(x => x.ClientTrips)
            .FirstOrDefaultAsync(x => x.IdClient == idClient);

        if (client == null)
        {
            throw new ClientNotFoundException("Client not found");
        }

        if (client.ClientTrips.Any())
        {
            throw new ClientAssignTripException("Client ma jeszcze wycieczki");
        }
        
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }

    public async Task AddClientToTripAsync(int idTrip, AddClientToTripDTO addClientToTrip)
    {
        var existingClient = await _context.Clients.FirstOrDefaultAsync(x => x.Pesel == addClientToTrip.Pesel);

        if (existingClient != null)
        {
            throw new ("Dodano klienta do wycieczki");
        }

        var trip = await _context.Trips
            .Include(x => x.ClientTrips)
            .FirstOrDefaultAsync(x => x.IdTrip == idTrip);

        if (trip == null)
        {
            throw new TripNotFoundException("Trip not found");
        }

        if (trip.DateFrom < DateTime.Now)
        {
            throw new Exception("Wycieczka już się odbyła");
        }
        
        var client = new Client
        {
            FirstName = addClientToTrip.FirstName,
            LastName = addClientToTrip.LastName,
            Pesel = addClientToTrip.Pesel,
            Email = addClientToTrip.Email,
            Telephone = addClientToTrip.Telephone,
        };
        
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = addClientToTrip.PaymentDate
        };
        
        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();
    }
}