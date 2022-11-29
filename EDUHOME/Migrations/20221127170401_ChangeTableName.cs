using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUHOME.Migrations
{
    public partial class ChangeTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FooterLogoAndSocials",
                table: "FooterLogoAndSocials");

            migrationBuilder.RenameTable(
                name: "FooterLogoAndSocials",
                newName: "FooterLogo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FooterLogo",
                table: "FooterLogo",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FooterLogo",
                table: "FooterLogo");

            migrationBuilder.RenameTable(
                name: "FooterLogo",
                newName: "FooterLogoAndSocials");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FooterLogoAndSocials",
                table: "FooterLogoAndSocials",
                column: "Id");
        }
    }
}
