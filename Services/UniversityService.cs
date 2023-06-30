using API.Contracts;
using API.DTOs.Universities;
using API.Models;

namespace API.Services;

public class UniversityService
{
    private readonly IUniversityRepository _universityRepository;

    public UniversityService(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public IEnumerable<UniversityDTO>? GetUniversity()
    {
        var universities = _universityRepository.GetAll();
        if (!universities.Any())
        {
            return null; // No universities found
        }

        var toDTO = universities.Select(university => new UniversityDTO
        {
            Guid = university.Guid,
            Code = university.Code,
            Name = university.Name
        }).ToList();

        return toDTO; // Universities found
    }

    public UniversityDTO? GetUniversity(Guid guid)
    {
        var university = _universityRepository.GetByGuid(guid);
        if (university is null)
        {
            return null; // No universities found
        }

        var toDTO = new UniversityDTO
        {
            Guid = university.Guid,
            Code = university.Code,
            Name = university.Name
        };

        return toDTO; // Universities found
    }

    public IEnumerable<UniversityDTO>? GetUniversity(string name)
    {
        var universities = _universityRepository.GetByName(name);
        if (!universities.Any())
        {
            return null; // No universities found
        }

        var toDTO = universities.Select(university => new UniversityDTO
        {
            Guid = university.Guid,
            Code = university.Code,
            Name = university.Name
        }).ToList();

        return toDTO; // Universities found
    }

    public UniversityDTO? CreateUniversity(NewUniversityDTO newUniversityDTO)
    {
        var university = new University
        {
            Code = newUniversityDTO.Code,
            Name = newUniversityDTO.Name,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdUniversity = _universityRepository.Create(university);
        if (createdUniversity is null)
        {
            return null; // University not created
        }

        var toDTO = new UniversityDTO
        {
            Guid = createdUniversity.Guid,
            Code = createdUniversity.Code,
            Name = createdUniversity.Name
        };

        return toDTO; // University created
    }

    public int UpdateUniversity(UniversityDTO universityDTO)
    {
        var isExist = _universityRepository.IsExist(universityDTO.Guid);
        if (!isExist)
        {
            return -1; // University not found
        }

        var getUniversity = _universityRepository.GetByGuid(universityDTO.Guid);

        var university = new University
        {
            Guid = universityDTO.Guid,
            Code = universityDTO.Code,
            Name = universityDTO.Name,
            ModifiedDate = DateTime.Now,
            CreatedDate = getUniversity!.CreatedDate
        };

        var isUpdate = _universityRepository.Update(university);
        if (!isUpdate)
        {
            return 0; // University not found
        }

        return 1;
    }

    public int DeleteUniversity(Guid guid)
    {
        var isExist = _universityRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // University not found
        }

        var university = _universityRepository.GetByGuid(guid);
        var isDelete = _universityRepository.Delete(university!);
        if (!isDelete)
        {
            return 0; // University not deleted
        }

        return 1;
    }
}
