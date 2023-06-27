using API.Contracts;
using API.Models;
using API.Repositories;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/roles")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _repository;

    public RoleController(IRoleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var roles = _repository.GetAll();

        if (!roles.Any())
        {
            return NotFound(new ResponseHandler<Role>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<Role>>
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
        var role = _repository.GetByGuid(guid);

        if (role is null)
        {
            return NotFound(new ResponseHandler<Role>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<Role>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = role
        });
    }

    [HttpPost]
    public IActionResult Create(Role role)
    {
        var createdRole = _repository.Create(role);
        if (createdRole is null)
        {
            return BadRequest(new ResponseHandler<Role>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created!"
            });
        }
        return Ok(new ResponseHandler<Role>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created"
        });
    }

    [HttpPut]
    public IActionResult Update(Role role)
    {
        var getGuid = (Guid)typeof(Role).GetProperty("Guid")!.GetValue(role!);
        var isFound = _repository.IsExist(getGuid);

        if (isFound is false)
        {
            return NotFound(new ResponseHandler<Role>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found!"
            });
        }

        var isUpdated = _repository.Update(role);

        if (!isUpdated)
        {
            return BadRequest(new ResponseHandler<Role>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data!"
            });
        }

        return Ok(new ResponseHandler<Role>
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
            return NotFound(new ResponseHandler<Role>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        var isDeleted = _repository.Delete(guid);

        if (!isDeleted)
        {
            return BadRequest(new ResponseHandler<Role>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<Role>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
