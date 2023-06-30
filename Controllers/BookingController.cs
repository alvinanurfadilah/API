using API.Contracts;
using API.DTOs.Bookings;
using API.Models;
using API.Repositories;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingController : ControllerBase
{
    private readonly BookingService _service;
    public BookingController(BookingService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var bookings = _service.GetBooking();

        if (!bookings.Any())
        {
            return NotFound(new ResponseHandler<BookingDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<BookingDTO>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = bookings
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var bookingDTO = _service.GetBooking(guid);

        if (bookingDTO is null)
        {
            return NotFound(new ResponseHandler<BookingDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<BookingDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = bookingDTO
        });
    }

    [HttpPost]
    public IActionResult Create(NewBookingDTO newBookingDTO)
    {
        var createdBooking = _service.CreateBooking(newBookingDTO);
        if (createdBooking is null)
        {
            return BadRequest(new ResponseHandler<BookingDTO>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created!"
            });
        }
        return Ok(new ResponseHandler<BookingDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created"
        });
    }

    [HttpPut]
    public IActionResult Update(BookingDTO bookingDTO)
    {
        var isUpdated = _service.UpdateBooking(bookingDTO);

        if (isUpdated is -1)
        {
            return NotFound(new ResponseHandler<BookingDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found!"
            });
        }

        if (isUpdated is 0)
        {
            return BadRequest(new ResponseHandler<BookingDTO>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data!"
            });
        }

        return Ok(new ResponseHandler<BookingDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _service.DeleteBooking(guid);

        if (isDeleted is -1)
        {
            return NotFound(new ResponseHandler<BookingDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (isDeleted is 0)
        {
            return BadRequest(new ResponseHandler<BookingDTO>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<BookingDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
