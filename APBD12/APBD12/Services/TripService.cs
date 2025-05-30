using APBD12.Data;
using APBD12.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace APBD12.Services;

public class TripService : ITripService
{
    private readonly Apbd12Context _context;

    public TripService(Apbd12Context context)
    {
        _context = context;
    }

    public async Task<GetTripsPageDto> GetTripPageAsync(int page = 1, int pageSize = 10)
    {
        var totalTrips = await _context.Trips.CountAsync();
        var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trpis = await _context.Trips
            .Include(x => x.ClientTrips)
            .ThenInclude(y => y.IdClientNavigation)
            .Include(x => x.IdCountries)
            .OrderByDescending(x => x.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new TripDto
            {
                Name = x.Name,
                Description = x.Description,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                MaxPeople = x.MaxPeople,
                Countries = x.IdCountries.Select(country => new CountryDto()
                {
                    Name = country.Name,
                }).ToList(),
                Clients = x.ClientTrips.Select(clientTrips => new ClientDto
                {
                    FirstName = clientTrips.IdClientNavigation.FirstName,
                    LastName = clientTrips.IdClientNavigation.LastName
                }).ToList()
            }).ToListAsync();

        return new GetTripsPageDto()
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = totalPages,
            Trips = trpis
        };
    }   
    
}