using CompanyBlazor.Shared.Models;
using CompanyBlazor5.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics;

namespace CompanyBlazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly CompanyBlazorDbContext _db;
        public DepartmentController(CompanyBlazorDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Department>>> GetDepartments()
        {
            return Ok(await _db.Departments.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _db.Departments.FirstOrDefaultAsync(h => h.departmentNo == id);
            if(department == null)
            {
                return NotFound($"Department {id} not found");
            }
            return Ok(department);
        }

        [HttpPost]
        public async Task<ActionResult<List<Department>>>CreateDepartment(Department entity)
        {
            if (entity.departmentName.Length <=20 && entity.departmentLocation.Length <= 20)
            {
                _db.Departments.Add(entity);
                await _db.SaveChangesAsync();
            }
            return Ok(await GetDbDepartments());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Department>>>UpdateDepartment(Department entity, int id)
        {
            var department = await _db.Departments.FirstOrDefaultAsync(dp => dp.departmentNo==id);
            if (department == null)
                return NotFound("Sorry, no department found");

            department.departmentName = entity.departmentName;
            department.departmentLocation = entity.departmentLocation;
            department.departmentNo = entity.departmentNo;

            await _db.SaveChangesAsync();

            return Ok(await GetDbDepartments());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Department>>> DeleteDepartment(int id)
        {
            var dbDepartment = await _db.Departments.FirstOrDefaultAsync(dp => dp.departmentNo==id);
            if (dbDepartment == null)
                return NotFound("Sorry, no department found");

            _db.Departments.Remove(dbDepartment);
            await _db.SaveChangesAsync();
      
            return Ok(await GetDbDepartments());
        }

        private async Task<List<Department>> GetDbDepartments()
        {
            return await _db.Departments.ToListAsync();
        }
    }
}
