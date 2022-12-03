using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUHOME.Migrations
{
    public partial class CreateContactsTableToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number2",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "Number1",
                table: "Contacts",
                newName: "WebsiteImageUrl");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Contacts",
                newName: "ContactNumberImageUrl");

            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "Contacts",
                newName: "ContactNumber");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdsressImageUrl",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FooterContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterContacts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FooterContacts");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "AdsressImageUrl",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "WebsiteImageUrl",
                table: "Contacts",
                newName: "Number1");

            migrationBuilder.RenameColumn(
                name: "ContactNumberImageUrl",
                table: "Contacts",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "ContactNumber",
                table: "Contacts",
                newName: "Adress");

            migrationBuilder.AddColumn<string>(
                name: "Number2",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
