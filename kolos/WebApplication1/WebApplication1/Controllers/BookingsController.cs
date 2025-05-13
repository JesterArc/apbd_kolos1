using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingByIdAsync(int id)
    {
        if (!await _bookingService.DoesBookingExistAsync(id))
        {
            return NotFound("No booking found for id = " + id);
        }
        BookingDTO booking = await _bookingService.GetBookingByIdAsync(id);
        return Ok(booking);
    }

    [HttpPost]
    public async Task<IActionResult> AddBookingAsync([FromBody] CreateBookingDTO booking)
    {
        if (await _bookingService.DoesBookingExistAsync(booking.bookingId))
        {
            return Conflict("Booking already exists");
        }

        if (!await _bookingService.DoesGuestExistAsync(booking.guestId))
        {
            return NotFound("Guest not found");
        }

        if (!await _bookingService.DoesEmployeeExistAsync(booking.employeeNumber))
        {
            return NotFound("Employee not found");
        }
        foreach (AttractionDTO attractionDto in booking.attractions )
            if (!await _bookingService.DoesAttractionExistAsync(attractionDto.Name))
            {
                return NotFound("Attraction not found");
            }

        try
        {
            await _bookingService.CreateBookingAsync(booking);
            foreach (var attraction in booking.attractions)
            {
                await _bookingService.AddBooking_AttractionAsync(booking.bookingId, attraction.Name, attraction.Amount);
            }
            return Ok("Booking created");
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error, booking was not created");
        }
    }
}

