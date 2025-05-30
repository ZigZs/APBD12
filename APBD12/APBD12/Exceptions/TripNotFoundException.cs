namespace APBD12.Exceptions;

public class TripNotFoundException : Exception
{
    public TripNotFoundException(string? message) : base(message)
    {
    }
}