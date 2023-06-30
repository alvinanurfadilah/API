using API.Contracts;
using API.DTOs.Rooms;
using API.Models;
using API.Repositories;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    private readonly RoomService _service;

    public RoomController(RoomService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var rooms = _service.GetRoom();

        if (!rooms.Any())
        {
            return NotFound(new ResponseHandler<RoomDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<RoomDTO>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = rooms
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var room = _service.GetRoom(guid);

        if (room is null)
        {
            return NotFound(new ResponseHandler<RoomDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<RoomDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = room
        });
    }

    [HttpPost]
    public IActionResult Create(NewRoomDTO newRoomDTO)
    {
        var createdRoom = _service.CreateRoom(newRoomDTO);
        if (createdRoom is null)
        {
            return BadRequest(new ResponseHandler<RoomDTO>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created!"
            });
        }
        return Ok(new ResponseHandler<RoomDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created"
        });
    }

    [HttpPut]
    public IActionResult Update(RoomDTO roomDTO)
    {
        var isUpdated = _service.UpdateRoom(roomDTO);

        if (isUpdated is -1)
        {
            return NotFound(new ResponseHandler<RoomDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found!"
            });
        }

        if (isUpdated is 0)
        {
            return BadRequest(new ResponseHandler<RoomDTO>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data!"
            });
        }

        return Ok(new ResponseHandler<RoomDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _service.DeleteRoom(guid);

        if (isDeleted is -1)
        {
            return NotFound(new ResponseHandler<RoomDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (isDeleted is 0)
        {
            return BadRequest(new ResponseHandler<RoomDTO>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<RoomDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
