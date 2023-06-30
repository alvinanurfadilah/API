using API.Contracts;
using API.DTOs.AccountRoles;
using API.Models;

namespace API.Services;

public class AccountRoleService
{
    private readonly IAccountRoleRepository _accountRoleRepository;

    public AccountRoleService(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    public IEnumerable<AccountRoleDTO>? GetAccountRole()
    {
        var accountRoles = _accountRoleRepository.GetAll();
        if (!accountRoles.Any())
        {
            return null; // No accountRoles found
        }

        var toDTO = accountRoles.Select(accountRole => new AccountRoleDTO
        {
            Guid = accountRole.Guid,
            AccountGuid = accountRole.AccountGuid,
            RoleGuid = accountRole.RoleGuid
        }).ToList();

        return toDTO; // AccountRoles found
    }

    public AccountRoleDTO? GetAccountRole(Guid guid)
    {
        var accountRole = _accountRoleRepository.GetByGuid(guid);
        if (accountRole is null)
        {
            return null; // No accountRoles found
        }

        var toDTO = new AccountRoleDTO
        {
            Guid = accountRole.Guid,
            AccountGuid = accountRole.AccountGuid,
            RoleGuid = accountRole.RoleGuid
        };

        return toDTO; // AccountRoles found
    }

    public AccountRoleDTO? CreateAccountRole(NewAccountRoleDTO newAccountRoleDTO)
    {
        var accountRole = new AccountRole
        {
            AccountGuid = newAccountRoleDTO.AccountGuid,
            RoleGuid = newAccountRoleDTO.RoleGuid,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdAccountRole = _accountRoleRepository.Create(accountRole);
        if (createdAccountRole is null)
        {
            return null; // AccountRole not created
        }

        var toDTO = new AccountRoleDTO
        {
            Guid = createdAccountRole.Guid,
            AccountGuid = createdAccountRole.AccountGuid,
            RoleGuid = createdAccountRole.RoleGuid
        };

        return toDTO; // AccountRole created
    }

    public int UpdateAccountRole(AccountRoleDTO accountRoleDTO)
    {
        var isExist = _accountRoleRepository.IsExist(accountRoleDTO.Guid);
        if (!isExist)
        {
            return -1; // AccountRole not found
        }

        var getAccountRole = _accountRoleRepository.GetByGuid(accountRoleDTO.Guid);

        var accountRole = new AccountRole
        {
            Guid = accountRoleDTO.Guid,
            AccountGuid = accountRoleDTO.AccountGuid,
            RoleGuid = accountRoleDTO.RoleGuid,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccountRole!.CreatedDate
        };

        var isUpdate = _accountRoleRepository.Update(accountRole);
        if (!isUpdate)
        {
            return 0; // AccountRole not found
        }

        return 1;
    }

    public int DeleteAccountRole(Guid guid)
    {
        var isExist = _accountRoleRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // AccountRole not found
        }

        var accountRole = _accountRoleRepository.GetByGuid(guid);
        var isDelete = _accountRoleRepository.Delete(accountRole!);
        if (!isDelete)
        {
            return 0; // AccountRole not deleted
        }

        return 1;
    }
}
