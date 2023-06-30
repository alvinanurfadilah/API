using API.Contracts;
using API.DTOs.Roles;
using API.Models;
using API.Repositories;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/roleDTOs")]
public class RoleDTOController : ControllerBase
{
    private readonly RoleService _service;

    public RoleDTOController(RoleService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var roles = _service.GetRole();

        if (!roles.Any())
        {
            return NotFound(new ResponseHandler<RoleDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<RoleDTO>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = roles
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var roleDTO = _service.GetRole(guid);

        if (roleDTO is null)
        {
            return NotFound(new ResponseHandler<RoleDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<RoleDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = roleDTO
        });
    }

    [HttpPost]
    public IActionResult Create(NewRoleDTO newRoleDTO)
    {
        var createdRoleDTO = _service.CreateRole(newRoleDTO);
        if (createdRoleDTO is null)
        {
            return BadRequest(new ResponseHandler<RoleDTO>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created!"
            });
        }
        return Ok(new ResponseHandler<RoleDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created"
        });
    }

    [HttpPut]
    public IActionResult Update(RoleDTO roleDTO)
    {
        var isUpdated = _service.UpdateRole(roleDTO);

        if (isUpdated is -1)
        {
            return NotFound(new ResponseHandler<RoleDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found!"
            });
        }

        if (isUpdated is 0)
        {
            return BadRequest(new ResponseHandler<RoleDTO>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data!"
            });
        }

        return Ok(new ResponseHandler<RoleDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _service.DeleteRole(guid);

        if (isDeleted is -1)
        {
            return NotFound(new ResponseHandler<RoleDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (isDeleted is 0)
        {
            return BadRequest(new ResponseHandler<RoleDTO>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<RoleDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
