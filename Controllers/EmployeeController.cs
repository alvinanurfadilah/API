using API.Contracts;
using API.Models;
using API.Repositories;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _repository;

    public EmployeeController(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var employees = _repository.GetAll();

        if (!employees.Any())
        {
            return NotFound(new ResponseHandler<Education>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<Employee>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = employees
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var employee = _repository.GetByGuid(guid);

        if (employee is null)
        {
            return NotFound(new ResponseHandler<Education>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found!"
            });
        }

        return Ok(new ResponseHandler<Employee>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = employee
        });
    }

    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        var createdEmployee = _repository.Create(employee);
        if (createdEmployee is null)
        {
            return BadRequest(new ResponseHandler<Employee>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created!"
            });
        }
        return Ok(new ResponseHandler<Employee>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created"
        });
    }

    [HttpPut]
    public IActionResult Update(Employee employee)
    {
        var getGuid = (Guid)typeof(Employee).GetProperty("Guid")!.GetValue(employee!);
        var isFound = _repository.IsExist(getGuid);

        if (isFound is false)
        {
            return NotFound(new ResponseHandler<Employee>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found!"
            });
        }

        var isUpdated = _repository.Update(employee);

        if (!isUpdated)
        {
            return BadRequest(new ResponseHandler<Employee>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data!"
            });
        }

        return Ok(new ResponseHandler<Employee>
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
            return NotFound(new ResponseHandler<Employee>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        var isDeleted = _repository.Delete(guid);

        if (!isDeleted)
        {
            return BadRequest(new ResponseHandler<Employee>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<Employee>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }
}
