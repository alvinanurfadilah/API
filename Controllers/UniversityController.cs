using API.Contracts;
using API.DTOs.Universities;
using API.Models;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/universities")]
public class UniversityController : ControllerBase
{
    private readonly UniversityService _service;

    public UniversityController(UniversityService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var universities = _service.GetUniversity();

        if (!universities.Any())
        {
            return NotFound(new ResponseHandler<UniversityDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<UniversityDTO>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = universities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var university = _service.GetUniversity(guid);

        if (university is null)
        {
            return NotFound(new ResponseHandler<UniversityDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<UniversityDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = university
        });
    }

    [HttpGet("get-by/{name}")]
    public IActionResult GetByName(string name)
    {
        var university = _service.GetUniversity(name);

        if (!university.Any())
        {
            return NotFound(new ResponseHandler<UniversityDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No universities found with the given name"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<UniversityDTO>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Universities found",
            Data = university
        });
    }

    [HttpPost]
    public IActionResult Create(NewUniversityDTO newUniversityDTO)
    {
        var createdUniversity = _service.CreateUniversity(newUniversityDTO);
        if (createdUniversity is null)
        {
            return BadRequest(new ResponseHandler<UniversityDTO>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created!"
            });
        }
        return Ok(new ResponseHandler<UniversityDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created"
        });
    }

    [HttpPut]
    public IActionResult Update(UniversityDTO universityDTO)
    {
        var isUpdated = _service.UpdateUniversity(universityDTO);

        if (isUpdated is -1)
        {
            return NotFound(new ResponseHandler<UniversityDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (isUpdated is 0)
        {
            return BadRequest(new ResponseHandler<UniversityDTO>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data!"
            });
        }

        return Ok(new ResponseHandler<UniversityDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _service.DeleteUniversity(guid);

        if (isDeleted is -1)
        {
            return NotFound(new ResponseHandler<UniversityDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }
        if (isDeleted is 0)
        {
            return BadRequest(new ResponseHandler<UniversityDTO>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<UniversityDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
