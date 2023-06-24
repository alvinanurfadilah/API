using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/universities")]
public class UniversityController : GeneralController<University>
{
    public UniversityController(IUniversityRepository repository) : base(repository) { }


    /*[HttpGet("{name}")]
    public IActionResult GetByName(string name)
    {
        var university = _repository.GetByName(name);

        if (university is null)
        {
            return NotFound();
        }

        return Ok(university);
    }*/
}
