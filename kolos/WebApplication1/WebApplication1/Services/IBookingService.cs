using WebApplication1.Models.DTOs;
namespace WebApplication1.Services;

public interface IBookingService
{
    public Task<BookingDTO> GetBookingByIdAsync(int id);
    public Task<bool> DoesBookingExistAsync(int id);
    public Task<bool> DoesGuestExistAsync(int id);
    public Task<bool> DoesEmployeeExistAsync(string number);
    public Task<bool> DoesAttractionExistAsync(string name);
    public Task CreateBookingAsync(CreateBookingDTO booking);
    public Task AddBooking_AttractionAsync(int id, string name, int amount);
    
}