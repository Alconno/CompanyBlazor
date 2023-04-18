using CompanyBlazor.Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyBlazor5.Client.Pages
{
    public class EmployeeDepartmentBase
    {
        public int employeeNo { get; set; }
        public string employeeName { get; set; }
        public int Salary { get; set; }
        public DateTime lastModifyDate { get; set; } // will be automatically set
        public int departmentNo { get; set; }
        public string departmentLocation_Name { get; set; }
    }
}
