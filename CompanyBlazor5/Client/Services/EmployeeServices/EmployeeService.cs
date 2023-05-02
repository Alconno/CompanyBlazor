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
            var res = await _http.GetAsync($"api/employee/{id}");

            if (res.IsSuccessStatusCode) {
                await SetEmployees(await _http.DeleteAsync($"api/employee/{id}"));
            }
        }

        public async Task GetDepartments()
        {
            var res = await _http.GetAsync("api/department");

            if (res.IsSuccessStatusCode)
                departments=await _http.GetFromJsonAsync<List<Department>>("api/department");
        }

        public async Task GetEmployees()
        {
            var res = await _http.GetAsync("api/employee");

            if (res.IsSuccessStatusCode)
                employees=await _http.GetFromJsonAsync<List<Employee>>("api/employee");
        }

        public async Task<Employee> GetEmployee(int id)
        {
            var res = await _http.GetAsync($"api/employee/{id}");

            if (res.IsSuccessStatusCode)
                return await _http.GetFromJsonAsync<Employee>($"api/employee/{id}");
            else
                return null;
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var res = await _http.GetAsync($"api/employee/{employee.employeeNo}");
            if (res.IsSuccessStatusCode){
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
