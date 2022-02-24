using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_FILE_VIEW_.Migrations
{
    public partial class SeedEmployeesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Hoppies", "Name" },
                values: new object[] { 1, 0, "Mohamed" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Hoppies", "Name" },
                values: new object[] { 2, 1, "Essam" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Hoppies", "Name" },
                values: new object[] { 3, 2, "Walid" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
