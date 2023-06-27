using API.Contracts;
using API.Models;
using API.Repositories;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _repository;

    public AccountController(IAccountRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var accounts = _repository.GetAll();

        if (!accounts.Any())
        {
            return NotFound(new ResponseHandler<Account>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<Account>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = accounts
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var account = _repository.GetByGuid(guid);

        if (account is null)
        {
            return NotFound(new ResponseHandler<Account>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<Account>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found"
            Data = account
        });
    }

    [HttpPost]
    public IActionResult Create(Account account)
    {
        var createdAccount = _repository.Create(account);
        if (createdAccount is null)
        {
            return BadRequest(new ResponseHandler<Account>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created!"
            });
        }
        return Ok(new ResponseHandler<Account>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created"
        });
    }

    [HttpPut]
    public IActionResult Update(Account account)
    {
        var getGuid = (Guid)typeof(Account).GetProperty("Guid")!.GetValue(account!);
        var isFound = _repository.IsExist(getGuid);

        if (isFound is false)
        {
            return NotFound(new ResponseHandler<Account>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found!"
            });
        }
        var isUpdated = _repository.Update(account);

        if (!isUpdated)
        {
            return BadRequest(new ResponseHandler<Account>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data!"
            });
        }

        return Ok(new ResponseHandler<Account>
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
            return NotFound(new ResponseHandler<Account>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        var isDeleted = _repository.Delete(guid);

        if (!isDeleted)
        {
            return BadRequest(new ResponseHandler<Account>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<Account>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
