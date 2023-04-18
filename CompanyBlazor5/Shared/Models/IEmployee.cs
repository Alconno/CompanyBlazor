using System;

namespace CompanyBlazor.Shared.Models
{
    public interface IEmployee
    {
        Department? department { get; set; }
        int departmentNo { get; set; }
        string employeeName { get; set; }
        int employeeNo { get; set; }
        DateTime lastModifyDate { get; set; }
        int Salary { get; set; }
    }
}