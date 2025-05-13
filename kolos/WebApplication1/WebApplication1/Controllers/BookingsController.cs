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
    public async Task<IActionResult> AddBookingAsync([FromBody] BookingDTO booking)
    {
        return Ok();
    }
}

