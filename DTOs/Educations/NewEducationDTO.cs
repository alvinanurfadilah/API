namespace API.DTOs.Educations;

public class NewEducationDTO
{
    public string Major { get; set; }
    public string Degree { get; set; }
    public float GPA { get; set; }
    public Guid UniversityGuid { get; set; }
}
