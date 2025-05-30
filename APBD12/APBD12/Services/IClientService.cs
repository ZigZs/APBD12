using APBD12.DTOs;
using APBD12.Models;

namespace APBD12.Services;

public interface IClientService
{
    Task DeleteClientAsync (int idClient);
    Task AddClientToTripAsync (int idTrip, AddClientToTripDTO addClientToTrip);
}