using API.Contracts;
using API.DTOs.Educations;
using API.Models;

namespace API.Services;

public class EducationService
{
    private readonly IEducationRepository _educationRepository;

    public EducationService(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }

    public IEnumerable<EducationDTO>? GetEducation()
    {
        var educations = _educationRepository.GetAll();
        if (!educations.Any())
        {
            return null; // No educations found
        }

        var toDTO = educations.Select(education => new EducationDTO
        {
            Guid = education.Guid,
            Major = education.Major,
            Degree = education.Degree,
            GPA = education.GPA,
            UniversityGuid = education.UniversityGuid
        }).ToList();

        return toDTO; // Educations found
    }

    public EducationDTO? GetEducation(Guid guid)
    {
        var education = _educationRepository.GetByGuid(guid);
        if (education is null)
        {
            return null; // No educations found
        }

        var toDTO = new EducationDTO
        {
            Guid = education.Guid,
            Major = education.Major,
            Degree = education.Degree,
            GPA = education.GPA,
            UniversityGuid = education.UniversityGuid
        };

        return toDTO; // Educations found
    }

    public EducationDTO? CreateEducation(NewEducationDTO newEducationDTO)
    {
        var education = new Education
        {
            Major = newEducationDTO.Major,
            Degree = newEducationDTO.Degree,
            GPA = newEducationDTO.GPA,
            UniversityGuid = newEducationDTO.UniversityGuid,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdEducation = _educationRepository.Create(education);
        if (createdEducation is null)
        {
            return null; // Education not created
        }

        var toDTO = new EducationDTO
        {
            Guid = createdEducation.Guid,
            Major = createdEducation.Major,
            Degree = createdEducation.Degree,
            GPA = createdEducation.GPA,
            UniversityGuid = createdEducation.UniversityGuid
        };

        return toDTO; // Education created
    }

    public int UpdateEducation(EducationDTO educationDTO)
    {
        var isExist = _educationRepository.IsExist(educationDTO.Guid);
        if (!isExist)
        {
            return -1; // Education not found
        }

        var getEducation = _educationRepository.GetByGuid(educationDTO.Guid);

        var education = new Education
        {
            Guid = educationDTO.Guid,
            Major = educationDTO.Major,
            Degree = educationDTO.Degree,
            GPA = educationDTO.GPA,
            UniversityGuid = educationDTO.UniversityGuid,
            ModifiedDate = DateTime.Now,
            CreatedDate = getEducation!.CreatedDate
        };

        var isUpdate = _educationRepository.Update(education);
        if (!isUpdate)
        {
            return 0; // Education not found
        }

        return 1;
    }

    public int DeleteEducation(Guid guid)
    {
        var isExist = _educationRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Education not found
        }

        var education = _educationRepository.GetByGuid(guid);
        var isDelete = _educationRepository.Delete(education!);
        if (!isDelete)
        {
            return 0; // Education not deleted
        }

        return 1;
    }
}
