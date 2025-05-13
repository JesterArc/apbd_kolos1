using Microsoft.Data.SqlClient;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Services;

public class BookingService : IBookingService
{
    private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=apbd;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
    public async Task<BookingDTO> GetBookingByIdAsync(int id)
    {
        BookingDTO? booking = null;
        GuestDTO? guest = null;
        EmployeeDTO? employee = null;
        // returns all info about trip specified by id plus name of country it takes place in
        var command = """
                      Select b.date, g.first_name, g.last_name, g.date_of_birth, e.first_name, e.last_name, e.employee_number, a.name, a.price, ba.amount
                      from Booking b
                      join dbo.Booking_Attraction BA on b.booking_id = BA.booking_id
                      join dbo.Attraction a on BA.attraction_id = a.attraction_id
                      join dbo.Employee E on b.employee_id = E.employee_id
                      join dbo.Guest G on b.guest_id = G.guest_id
                      where b.booking_id = @id
                      """;
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();
            cmd.Parameters.AddWithValue("@id", id);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                
                while (await reader.ReadAsync())
                {
                    // there is a validator method called before this one so the trip can only be null if
                    // this is the first iteration
                    if (guest == null)
                    {
                        guest = new GuestDTO()
                        {
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            DateOfBirth = reader.GetDateTime(3)
                        };
                    }

                    if (employee == null)
                    {
                        employee = new EmployeeDTO()
                        {
                            FirstName = reader.GetString(4),
                            LastName = reader.GetString(5),
                            EmployeeNumber = reader.GetString(6)
                        };
                    }

                    if (booking == null)
                    {
                        booking = new BookingDTO()
                        {
                            date = reader.GetDateTime(0),
                            guest = guest,
                            employee = employee,
                            attractions = new List<AttractionDTO>()
                        };
                    }
                    
                    String name = reader.GetString(7);
                    var attraction = booking.attractions.FirstOrDefault(e => e.Name == name);
                    // if this country is not already in our list of countries for the trip
                    if (attraction is null)
                    {
                        attraction = new AttractionDTO()
                        {
                            Name = name,
                            Price = reader.GetDecimal(8),
                            Amount = reader.GetInt32(9)
                        };
                        booking.attractions.Add(attraction);
                    }
                }
            }
        }
        return booking;
    }

    public async Task<bool> DoesBookingExistAsync(int id)
    {
        int quantity = 0;

        var command = $"Select Count(1) from Booking where booking_id = @id";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();
            cmd.Parameters.AddWithValue("@id", id);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    quantity = reader.GetInt32(0);
                }
            }
        }
        return quantity > 0;
    }
}