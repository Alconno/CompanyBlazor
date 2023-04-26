using CompanyBlazor.Shared.Models;
using CompanyBlazor5.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace CompanyBlazor5.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly CompanyBlazorDbContext _db;

        public EmployeeController(CompanyBlazorDbContext db)
        {
            _db=db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployees()
        {
            var employees = await _db.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpGet("departments")]
        public async Task<ActionResult<List<Department>>> GetDepartments()
        {
            var departments = await _db.Departments.ToListAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = _db.Employees
                .Include(h => h.department)
                .FirstOrDefault(h => h.employeeNo==id);
            if (employee==null)
            {
                return NotFound("Sorry, no employee found\n");
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee entity)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            entity.department = null;
            _db.Employees.Add(entity);
            await _db.SaveChangesAsync();

            return Ok(await GetDbEmployees());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(Employee entity, int id)
        {
            var dbEmployee = await _db.Employees
                        .Include(sh => sh.department)
                        .FirstOrDefaultAsync(sh => sh.employeeNo==id);
            if (dbEmployee==null) return NotFound("sorry, employee not found\n");

            dbEmployee.employeeName = entity.employeeName;
            dbEmployee.Salary = entity.Salary;
            dbEmployee.lastModifyDate = DateTime.Now;
            dbEmployee.departmentNo = entity.departmentNo;
            await _db.SaveChangesAsync();

            return Ok(await GetDbEmployees());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Employee>>> DeleteEmployee(int id)
        {
            var dbEmployee = await _db.Employees
                        .Include(sh => sh.department)
                        .FirstOrDefaultAsync(sh => sh.employeeNo==id);
            if (dbEmployee==null) return NotFound("sorry, employee not found\n");

            _db.Employees.Remove(dbEmployee);
            await _db.SaveChangesAsync();

            return Ok(await GetDbEmployees());
        }


        private async Task<List<Employee>> GetDbEmployees()
        {
            return await _db.Employees.Include(sh => sh.department).ToListAsync();
        }
    }
}