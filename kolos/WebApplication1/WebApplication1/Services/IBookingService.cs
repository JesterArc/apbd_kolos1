using WebApplication1.Models.DTOs;
namespace WebApplication1.Services;

public interface IBookingService
{
    public Task<BookingDTO> GetBookingByIdAsync(int id);
    public Task<bool> DoesBookingExistAsync(int id);
}