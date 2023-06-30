using API.Contracts;
using API.DTOs.Roles;
using API.DTOs.Rooms;
using API.Models;

namespace API.Services;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public IEnumerable<RoleDTO>? GetRole()
    {
        var roles = _roleRepository.GetAll();
        if (!roles.Any())
        {
            return null; // No roles found
        }

        var toDTO = roles.Select(role => new RoleDTO
        {
            Guid = role.Guid,
            Name = role.Name
        }).ToList();

        return toDTO; // Roles found
    }

    public RoleDTO? GetRole(Guid guid)
    {
        var role = _roleRepository.GetByGuid(guid);
        if (role is null)
        {
            return null; // No Roles found
        }

        var toDTO = new RoleDTO
        {
            Guid = role.Guid,
            Name = role.Name
        };

        return toDTO; // Roles found
    }

    public RoleDTO? CreateRole(NewRoleDTO newRoleDTO)
    {
        var role = new Role
        {
            Name = newRoleDTO.Name,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdRole = _roleRepository.Create(role);
        if (createdRole is null)
        {
            return null; // Role not created
        }

        var toDTO = new RoleDTO
        {
            Guid = createdRole.Guid,
            Name = createdRole.Name
        };

        return toDTO; // Role created
    }

    public int UpdateRole(RoleDTO roleDTO)
    {
        var isExist = _roleRepository.IsExist(roleDTO.Guid);
        if (!isExist)
        {
            return -1; // Role not found
        }

        var getRole = _roleRepository.GetByGuid(roleDTO.Guid);

        var role = new Role
        {
            Guid = roleDTO.Guid,
            Name = roleDTO.Name,
            ModifiedDate = DateTime.Now,
            CreatedDate = getRole!.CreatedDate
        };

        var isUpdate = _roleRepository.Update(role);
        if (!isUpdate)
        {
            return 0; // Role not found
        }
        return 1;
    }

    public int DeleteRole(Guid guid)
    {
        var isExist = _roleRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Role not found
        }

        var role = _roleRepository.GetByGuid(guid);
        var isDelete = _roleRepository.Delete(role!);
        if (!isDelete)
        {
            return 0; // Role not deleted
        }

        return 1;
    }
}
