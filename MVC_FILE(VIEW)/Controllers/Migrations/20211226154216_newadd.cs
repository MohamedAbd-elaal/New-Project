using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_FILE_VIEW_.Migrations
{
    public partial class newadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "photopath",
                table: "Employees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photopath",
                table: "Employees");
        }
    }
}
