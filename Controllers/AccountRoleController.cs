using API.Contracts;
using API.DTOs.AccountRoles;
using API.DTOs.Accounts;
using API.Models;
using API.Repositories;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/accountRoles")]
public class AccountRoleController : ControllerBase
{
    private readonly AccountRoleService _service;

    public AccountRoleController(AccountRoleService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var accountRoles = _service.GetAccountRole();

        if (!accountRoles.Any())
        {
            return NotFound(new ResponseHandler<AccountRoleDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<AccountRoleDTO>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = accountRoles
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var accountRole = _service.GetAccountRole(guid);

        if (accountRole is null)
        {
            return NotFound(new ResponseHandler<AccountRoleDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!",
                Data = accountRole
            });
        }

        return Ok(new ResponseHandler<AccountRoleDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found"
        });
    }

    [HttpPost]
    public IActionResult Create(NewAccountRoleDTO newAccountRoleDTO)
    {
        var createdAccountRole = _service.CreateAccountRole(newAccountRoleDTO);
        if (createdAccountRole is null)
        {
            return BadRequest(new ResponseHandler<AccountRole>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created!"
            });
        }
        return Ok(new ResponseHandler<AccountRoleDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created"
        });
    }

    [HttpPut]
    public IActionResult Update(AccountRoleDTO accountRoleDTO)
    {
        var isUpdated = _service.UpdateAccountRole(accountRoleDTO);

        if (isUpdated is -1)
        {
            return NotFound(new ResponseHandler<AccountRoleDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (isUpdated is 0)
        {
            return BadRequest(new ResponseHandler<AccountRoleDTO>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data!"
            });
        }

        return Ok(new ResponseHandler<AccountRoleDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _service.DeleteAccountRole(guid);

        if (isDeleted is -1)
        {
            return NotFound(new ResponseHandler<AccountRoleDTO>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (isDeleted is 0)
        {
            return BadRequest(new ResponseHandler<AccountRoleDTO>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<AccountRoleDTO>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
