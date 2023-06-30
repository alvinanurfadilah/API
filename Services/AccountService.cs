using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.Universities;
using API.Models;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public IEnumerable<AccountDTO>? GetAccount()
    {
        var accounts = _accountRepository.GetAll();
        if (!accounts.Any())
        {
            return null; // No accounts found
        }

        var toDTO = accounts.Select(account => new AccountDTO
        {
            Guid = account.Guid,
            Password = account.Password,
            IsDeleted = account.IsDeleted,
            OTP = account.OTP,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime
        }).ToList();

        return toDTO; // Accounts found
    }

    public AccountDTO? GetAccount(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null)
        {
            return null; // No accounts found
        }

        var toDTO = new AccountDTO
        {
            Guid = account.Guid,
            Password = account.Password,
            IsDeleted = account.IsDeleted,
            OTP = account.OTP,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime
        };

        return toDTO; // Accounts found
    }

    public AccountDTO? CreateAccount(NewAccountDTO newAccountDTO)
    {
        var account = new Account
        {
            Password = newAccountDTO.Password,
            IsDeleted = newAccountDTO.IsDeleted,
            OTP = newAccountDTO.OTP,
            IsUsed = newAccountDTO.IsUsed,
            ExpiredTime = newAccountDTO.ExpiredTime,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdAccount = _accountRepository.Create(account);
        if (createdAccount is null)
        {
            return null; // Account not created
        }

        var toDTO = new AccountDTO
        {
            Guid = createdAccount.Guid,
            Password = createdAccount.Password,
            IsDeleted = createdAccount.IsDeleted,
            OTP = createdAccount.OTP,
            IsUsed = createdAccount.IsUsed,
            ExpiredTime = createdAccount.ExpiredTime
        };

        return toDTO; // Account created
    }

    public int UpdateAccount(AccountDTO accountDTO)
    {
        var isExist = _accountRepository.IsExist(accountDTO.Guid);
        if (!isExist)
        {
            return -1; // Account not found
        }

        var getAccount = _accountRepository.GetByGuid(accountDTO.Guid);

        var account = new Account
        {
            Guid = accountDTO.Guid,
            Password = accountDTO.Password,
            IsDeleted = accountDTO.IsDeleted,
            OTP = accountDTO.OTP,
            IsUsed = accountDTO.IsUsed,
            ExpiredTime = accountDTO.ExpiredTime,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccount!.CreatedDate
        };

        var isUpdate = _accountRepository.Update(account);
        if (!isUpdate)
        {
            return 0; // Account not found
        }

        return 1;
    }

    public int DeleteAccount(Guid guid)
    {
        var isExist = _accountRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Account not found
        }

        var account = _accountRepository.GetByGuid(guid);
        var isDelete = _accountRepository.Delete(account!);
        if (!isDelete)
        {
            return 0; // Account not deleted
        }

        return 1;
    }
}
