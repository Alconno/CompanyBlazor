using CompanyBlazor.Shared.Models;

namespace CompanyBlazor5.Client.Services.DepartmentServices
{
    public interface IDepartmentService
    {
        List<Department> departments { get; set; }

        Task getDepartments();

        Task<Department> getDepartment(int id);

        Task CreateDepartment(Department entity);
        Task UpdateDepartment(Department entity);
        Task DeleteDepartment(int id);
    }
}
