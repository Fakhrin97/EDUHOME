using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUHOME.Migrations
{
    public partial class RemoveSocialMediaIconFromFooterLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SocialMediaIcon",
                table: "FooterLogoAndSocials");

            migrationBuilder.DropColumn(
                name: "SocialMediaLink",
                table: "FooterLogoAndSocials");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SocialMediaIcon",
                table: "FooterLogoAndSocials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SocialMediaLink",
                table: "FooterLogoAndSocials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
