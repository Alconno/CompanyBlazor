using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyBlazor5.Server.Migrations
{
    /// <inheritdoc />
    public partial class yeyeyse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    departmentNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    departmentName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    departmentLocation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.departmentNo);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    loginNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loginUserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    loginPassword = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.loginNo);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    employeeNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employeeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    lastModifyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    departmentNo1 = table.Column<int>(type: "int", nullable: true),
                    departmentNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.employeeNo);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_departmentNo1",
                        column: x => x.departmentNo1,
                        principalTable: "Departments",
                        principalColumn: "departmentNo",
                        onDelete: ReferentialAction.Cascade); // very important thingy <-- ( if Department that was part of employee is deleted, so is employee deleted )
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "departmentNo", "departmentLocation", "departmentName" },
                values: new object[,]
                {
                    { 1, "London", "Development" },
                    { 2, "Zurich", "Development" },
                    { 3, "Osijek", "Development" },
                    { 4, "London", "Sales" },
                    { 5, "Zurich", "Sales" },
                    { 6, "Osijek", "Sales" },
                    { 7, "Basel", "Sales" },
                    { 8, "Lugano", "Sales" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "employeeNo", "Salary", "departmentNo", "departmentNo1", "employeeName", "lastModifyDate" },
                values: new object[,]
                {
                    { 1, 50000, 4, null, "Fred Davies", new DateTime(2023, 4, 10, 3, 11, 20, 681, DateTimeKind.Local).AddTicks(6272) },
                    { 2, 50000, 3, null, "Bernard Katic", new DateTime(2023, 4, 10, 3, 11, 20, 681, DateTimeKind.Local).AddTicks(6272) },
                    { 3, 30000, 5, null, "Rich Davies", new DateTime(2023, 4, 10, 3, 11, 20, 681, DateTimeKind.Local).AddTicks(6272) },
                    { 4, 30000, 6, null, "Eva Dobos", new DateTime(2023, 4, 10, 3, 11, 20, 681, DateTimeKind.Local).AddTicks(6272) },
                    { 5, 25000, 8, null, "Mario Hunjadi", new DateTime(2023, 4, 10, 3, 11, 20, 681, DateTimeKind.Local).AddTicks(6272) },
                    { 6, 25000, 7, null, "Jean Michele", new DateTime(2023, 4, 10, 3, 11, 20, 681, DateTimeKind.Local).AddTicks(6272) },
                    { 7, 25000, 1, null, "Bill Gates", new DateTime(2023, 4, 10, 3, 11, 20, 681, DateTimeKind.Local).AddTicks(6272) },
                    { 8, 30000, 3, null, "Maja Janic", new DateTime(2023, 4, 10, 3, 11, 20, 681, DateTimeKind.Local).AddTicks(6272) },
                    { 9, 35000, 3, null, "Igor Horvat", new DateTime(2023, 4, 10, 3, 11, 20, 681, DateTimeKind.Local).AddTicks(6272) }
                });

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "loginNo", "loginPassword", "loginUserName" },
                values: new object[,]
                {
                    { 1, new Encryption.Encrypt().Hash("ItsNotSoft"), "Bill" },
                    { 2, new Encryption.Encrypt().Hash("trollsRule"), "Jean" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_departmentNo1",
                table: "Employees",
                column: "departmentNo1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
