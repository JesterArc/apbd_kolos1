namespace WebApplication1.Models.DTOs;

public class CreateBookingDTO
{
    public required int bookingId { get; set; }
    public required int guestId {get; set;}
    public required string employeeNumber { get; set; }
    public required List<AttractionDTO> attractions { get; set; }
}