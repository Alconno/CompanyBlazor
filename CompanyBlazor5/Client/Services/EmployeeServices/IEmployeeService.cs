using CompanyBlazor.Shared.Models;

namespace CompanyBlazor5.Client.Services.EmployeeServices
{
    public interface IEmployeeService
    {
        List<Employee> employees { get; set; }
        List<Department> departments { get; set; }
        Task GetDepartments();
        Task GetEmployees();
        Task<Employee> GetSingleEmployee(int id);

        Task CreateEmployee(Employee employee);
        Task UpdateEmployee(Employee employee); 
        Task DeleteEmployee(int id);
    }
}
