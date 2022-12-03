using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUHOME.Migrations
{
    public partial class UpdateCoumnNameAddresss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdsressImageUrl",
                table: "Contacts",
                newName: "AddressImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddressImageUrl",
                table: "Contacts",
                newName: "AdsressImageUrl");
        }
    }
}
