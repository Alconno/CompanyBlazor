using CompanyBlazor.Shared.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Net.Http.Json;

namespace CompanyBlazor5.Client.Services.DepartmentServices
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;
        public DepartmentService(HttpClient http, NavigationManager navigationManager)
        {
            _http=http;
            _navigationManager=navigationManager;
        }

        public List<Department> departments { get; set; } = new List<Department>();

        public async Task getDepartments()
        {
            var res = await _http.GetAsync("api/department");
            
            if (res.IsSuccessStatusCode)
                departments = await _http.GetFromJsonAsync<List<Department>>("api/department");
        }

        public async Task<Department> getDepartment(int id)
        {
            var res = await _http.GetAsync($"api/department/{id}");

            if (res.IsSuccessStatusCode)
                return await _http.GetFromJsonAsync<Department>($"api/department/{id}");
            else
                return null;
        }

        public async Task CreateDepartment(Department entity)
        {
            var result = await _http.PostAsJsonAsync("api/department", entity);
            await SetDepartments(result);
        }

        public async Task UpdateDepartment(Department entity)
        {
            var res = await _http.GetAsync($"api/department/{entity.departmentNo}");

            if (res.IsSuccessStatusCode)
            {
                var result = await _http.PutAsJsonAsync($"api/department/{entity.departmentNo}", entity);
                await SetDepartments(result);
            }
        }

        public async Task DeleteDepartment(int id)
        {
            var res = await _http.GetAsync($"api/department/{id}");

            if (res.IsSuccessStatusCode)
            {
                var result = await _http.DeleteAsync($"api/department/{id}");
                await SetDepartments(result);
            }
        }

        private async Task SetDepartments(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<Department>>();
            departments = response;
            _navigationManager.NavigateTo("departments");   
        }


    }
}
