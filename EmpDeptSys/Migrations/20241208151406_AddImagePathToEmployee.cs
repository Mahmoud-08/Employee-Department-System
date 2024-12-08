using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpDeptSys.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Employees",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Employees",
                newName: "Image");
        }
    }
}
