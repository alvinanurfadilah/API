using API.Contracts;
using API.DTOs.Educations;
using API.Models;
using API.Repositories;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/educations")]
public class EducationController : ControllerBase
{
    private readonly EducationService _service;

    public EducationController(EducationService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var educations = _service.GetEducation();

        if (!educations.Any())
        {
            return NotFound(new ResponseHandler<EducationDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<EducationDTO>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = educations
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var education = _service.GetEducation(guid);

        if (education is null)
        {
            return NotFound(new ResponseHandler<EducationDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<EducationDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = education
        });
    }

    [HttpPost]
    public IActionResult Create(NewEducationDTO newEducationDTO)
    {
        var createdEducation = _service.CreateEducation(newEducationDTO);
        if (createdEducation is null)
        {
            return BadRequest(new ResponseHandler<EducationDTO>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created!"
            });
        }

        return Ok(new ResponseHandler<EducationDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created"
        });
    }

    [HttpPut]
    public IActionResult Update(EducationDTO educationDTO)
    {
        var isUpdated = _service.UpdateEducation(educationDTO);

        if (isUpdated is -1)
        {
            return NotFound(new ResponseHandler<EducationDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found!"
            });
        }

        if (isUpdated is 0)
        {
            return BadRequest(new ResponseHandler<EducationDTO>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data!"
            });
        }

        return Ok(new ResponseHandler<EducationDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _service.DeleteEducation(guid);

        if (isDeleted is -1)
        {
            return NotFound(new ResponseHandler<EducationDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (isDeleted is 0)
        {
            return BadRequest(new ResponseHandler<EducationDTO>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<EducationDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
