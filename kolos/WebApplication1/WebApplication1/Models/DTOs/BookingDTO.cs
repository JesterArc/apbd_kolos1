namespace WebApplication1.Models.DTOs;

public class BookingDTO
{
    public required DateTime date { get; set; }
    public GuestDTO guest { get; set; }
    public EmployeeDTO employee { get; set; }
    public List<AttractionDTO> attractions { get; set; }
}