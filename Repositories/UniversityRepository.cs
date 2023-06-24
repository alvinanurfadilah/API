using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(BookingDbContext context) : base(context) { }

    public IEnumerable<University> GetByName(string name)
    {
        return _context.Set<University>().Where(university => university.Name.Contains(name));
    }
}
