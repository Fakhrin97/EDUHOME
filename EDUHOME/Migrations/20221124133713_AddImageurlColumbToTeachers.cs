using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUHOME.Migrations
{
    public partial class AddImageurlColumbToTeachers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imageurl",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imageurl",
                table: "Teachers");
        }
    }
}
