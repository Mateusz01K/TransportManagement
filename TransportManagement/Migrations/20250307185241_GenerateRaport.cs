using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportManagement.Migrations
{
    public partial class GenerateRaport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DriverEmail",
                table: "Finances",
                newName: "EmployeeEmail");

            migrationBuilder.RenameColumn(
                name: "DriverEmail",
                table: "FinanceReports",
                newName: "Role");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeEmail",
                table: "FinanceReports",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "NetProfit",
                table: "FinanceReports",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeEmail",
                table: "FinanceReports");

            migrationBuilder.DropColumn(
                name: "NetProfit",
                table: "FinanceReports");

            migrationBuilder.RenameColumn(
                name: "EmployeeEmail",
                table: "Finances",
                newName: "DriverEmail");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "FinanceReports",
                newName: "DriverEmail");
        }
    }
}
