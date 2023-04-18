using CompanyBlazor.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Runtime.Intrinsics.Arm;

namespace CompanyBlazor5.Server.Data
{
    public class CompanyBlazorDbContext : DbContext
    {
        public CompanyBlazorDbContext(DbContextOptions<CompanyBlazorDbContext> options) : base(options)
        {

        }

        public List<Department> dps = new List<Department> {
                new Department() { departmentNo=1, departmentName="Development", departmentLocation="London" },
                new Department() { departmentNo=2, departmentName="Development", departmentLocation="Zurich" },
                new Department() { departmentNo=3, departmentName="Development", departmentLocation="Osijek" },
                new Department() { departmentNo=4, departmentName="Sales", departmentLocation="London" },
                new Department() { departmentNo=5, departmentName="Sales", departmentLocation="Zurich" },
                new Department() { departmentNo=6, departmentName="Sales", departmentLocation="Osijek" },
                new Department() { departmentNo=7, departmentName="Sales", departmentLocation="Basel" },
                new Department() { departmentNo=8, departmentName="Sales", departmentLocation="Lugano" }
            };

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Login> Logins { get; set; }

        public static DateTime time = DateTime.Now;

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<Login>().ToTable("Logins");

            modelBuilder.Entity<Login>().HasData(
                new Login { loginNo=1, loginUserName="Bill", loginPassword="ItsNotSoft" },
                new Login { loginNo=2, loginUserName="Jean", loginPassword="trollsRule" }
                );


            List<Employee> empls = new List<Employee>
            {
                new Employee() { employeeNo=1, employeeName="Fred Davies", Salary=50000, lastModifyDate=time, departmentNo=4,  },
                new Employee() { employeeNo=2, employeeName="Bernard Katic", Salary=50000, lastModifyDate=time, departmentNo=3,},
                new Employee() { employeeNo=3, employeeName="Rich Davies", Salary=30000, lastModifyDate=time, departmentNo=5,  },
                new Employee() { employeeNo=4, employeeName="Eva Dobos", Salary=30000, lastModifyDate=time, departmentNo=6, },
                new Employee() { employeeNo=5, employeeName="Mario Hunjadi", Salary=25000, lastModifyDate=time, departmentNo=8, },
                new Employee() { employeeNo=6, employeeName="Jean Michele", Salary=25000, lastModifyDate=time, departmentNo=7, },
                new Employee() { employeeNo=7, employeeName="Bill Gates", Salary=25000, lastModifyDate=time, departmentNo=1, },
                new Employee() { employeeNo=8, employeeName="Maja Janic", Salary=30000, lastModifyDate=time,departmentNo=3, },
                new Employee() { employeeNo=9, employeeName="Igor Horvat", Salary=35000, lastModifyDate=time, departmentNo=3, }
            };

            modelBuilder.Entity<Department>().HasData(dps);
            modelBuilder.Entity<Employee>().HasData(empls);

           }
    }
}
