using API.Contracts;
using API.Models;
using API.Repositories;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _repository;

    public RoomController(IRoomRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var rooms = _repository.GetAll();

        if (!rooms.Any())
        {
            return NotFound(new ResponseHandler<Room>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<Room>>
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
        var room = _repository.GetByGuid(guid);

        if (room is null)
        {
            return NotFound(new ResponseHandler<Room>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<Room>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = room
        });
    }

    [HttpPost]
    public IActionResult Create(Room room)
    {
        var createdRoom = _repository.Create(room);
        if (createdRoom is null)
        {
            return BadRequest(new ResponseHandler<Room>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created!"
            });
        }
        return Ok(new ResponseHandler<Room>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created"
        });
    }

    [HttpPut]
    public IActionResult Update(Room room)
    {
        var getGuid = (Guid)typeof(Room).GetProperty("Guid")!.GetValue(room!);
        var isFound = _repository.IsExist(getGuid);

        if (isFound is false)
        {
            return NotFound(new ResponseHandler<Room>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found!"
            });
        }

        var isUpdated = _repository.Update(room);

        if (!isUpdated)
        {
            return BadRequest(new ResponseHandler<Room>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data!"
            });
        }

        return Ok(new ResponseHandler<Room>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isFound = _repository.IsExist(guid);

        if (isFound is false)
        {
            return NotFound(new ResponseHandler<Room>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        var isDeleted = _repository.Delete(guid);

        if (!isDeleted)
        {
            return BadRequest(new ResponseHandler<Room>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<Room>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
