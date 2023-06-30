using API.Contracts;
using API.DTOs.Employees;
using API.Models;

namespace API.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public IEnumerable<EmployeeDTO>? GetEmployee()
    {
        var employees = _employeeRepository.GetAll();
        if (!employees.Any())
        {
            return null; // No employees found
        }

        var toDTO = employees.Select(employee => new EmployeeDTO
        {
            Guid = employee.Guid,
            NIK = employee.NIK,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber
        }).ToList();

        return toDTO; // Employees found
    }

    public EmployeeDTO? GetEmployee(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee is null)
        {
            return null; // No employees found
        }

        var toDTO = new EmployeeDTO
        {
            Guid = employee.Guid,
            NIK = employee.NIK,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber
        };

        return toDTO; // Employees found
    }

    public EmployeeDTO? CreateEmployee(NewEmployeeDTO newEmployeeDTO)
    {
        var employee = new Employee
        {
            NIK = newEmployeeDTO.NIK,
            FirstName = newEmployeeDTO.FirstName,
            LastName = newEmployeeDTO.LastName,
            BirthDate = newEmployeeDTO.BirthDate,
            Gender = newEmployeeDTO.Gender,
            HiringDate = DateTime.Now,
            Email = newEmployeeDTO.Email,
            PhoneNumber = newEmployeeDTO.PhoneNumber,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdEmployee = _employeeRepository.Create(employee);
        if (createdEmployee is null)
        {
            return null; // Employee not created
        }

        var toDTO = new EmployeeDTO
        {
            Guid = createdEmployee.Guid,
            NIK = createdEmployee.NIK,
            FirstName = createdEmployee.FirstName,
            LastName = createdEmployee.LastName,
            BirthDate = createdEmployee.BirthDate,
            Gender = createdEmployee.Gender,
            HiringDate = createdEmployee.HiringDate,
            Email = createdEmployee.Email,
            PhoneNumber = createdEmployee.PhoneNumber
        };

        return toDTO; // Employee created
    }

    public int UpdateEmployee(EmployeeDTO employeeDTO)
    {
        var isExist = _employeeRepository.IsExist(employeeDTO.Guid);
        if (!isExist)
        {
            return -1; // Employee not found
        }

        var getEmployee = _employeeRepository.GetByGuid(employeeDTO.Guid);

        var employee = new Employee
        {
            Guid = employeeDTO.Guid,
            NIK = employeeDTO.NIK,
            FirstName = employeeDTO.FirstName,
            LastName = employeeDTO.LastName,
            BirthDate = employeeDTO.BirthDate,
            Gender = employeeDTO.Gender,
            HiringDate = employeeDTO.HiringDate,
            Email = employeeDTO.Email,
            PhoneNumber = employeeDTO.PhoneNumber,
            ModifiedDate = DateTime.Now,
            CreatedDate = getEmployee!.CreatedDate
        };

        var isUpdate = _employeeRepository.Update(employee);
        if (!isUpdate)
        {
            return 0; // Employee not found
        }

        return 1;
    }

    public int DeleteEmployee(Guid guid)
    {
        var isExist = _employeeRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Employee not found
        }

        var employee = _employeeRepository.GetByGuid(guid);
        var isDelete = _employeeRepository.Delete(employee!);
        if (!isDelete)
        {
            return 0; // Employee not deleted
        }

        return 1;
    }
}
