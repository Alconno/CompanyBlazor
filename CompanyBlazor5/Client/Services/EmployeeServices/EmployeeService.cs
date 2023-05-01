using CompanyBlazor.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace CompanyBlazor5.Client.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;
        
        public EmployeeService(HttpClient httpClient, NavigationManager navigationManager)
        {
            _http=httpClient;     
            _navigationManager=navigationManager;
        }

        public List<Employee> employees { get; set; } = new List<Employee>();
        public List<Department> departments { get; set; } = new List<Department>();

        public async Task CreateEmployee(Employee employee)
        {
            var result = await _http.PostAsJsonAsync("api/employee", employee);
            await SetEmployees(result);
        }

        public async Task DeleteEmployee(int id)
        {
            if (await _http.GetFromJsonAsync<Employee>($"api/employee/{id}") != null) {
                var result = await _http.DeleteAsync($"api/employee/{id}");
                await SetEmployees(result);
            }
        }

        public async Task GetDepartments()
        {
            var result = await _http.GetFromJsonAsync<List<Department>>("api/department");
            if (result != null)
                departments=result;
        }

        public async Task GetEmployees()
        {
            var result = await _http.GetFromJsonAsync<List<Employee>>("api/employee");
            if (result != null)
                employees=result;
        }

        public async Task<Employee> GetSingleEmployee(int id)
        {
            var res = await _http.GetAsync($"api/employee/{id}");

            if (res.IsSuccessStatusCode)
                return await _http.GetFromJsonAsync<Employee>($"api/employee/{id}");
            else
                return null;
        }

        public async Task UpdateEmployee(Employee employee)
        {
            if (await _http.GetFromJsonAsync<Employee>($"api/employee/{employee.employeeNo}") != null){
                var result = await _http.PutAsJsonAsync($"api/employee/{employee.employeeNo}", employee);
                await SetEmployees(result);
            }
        }

        private async Task SetEmployees(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<Employee>>();
            employees = response;
            _navigationManager.NavigateTo("employees");
        }
    }
}
