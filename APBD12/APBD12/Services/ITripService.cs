using APBD12.DTOs;

namespace APBD12.Services;

public interface ITripService
{
    Task<GetTripsPageDto> GetTripPageAsync(int page = 1, int pageSize = 10);
}