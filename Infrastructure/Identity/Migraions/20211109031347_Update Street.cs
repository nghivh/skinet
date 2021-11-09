using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Identity.Migraions
{
    public partial class UpdateStreet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stress",
                table: "Address",
                newName: "Street");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Address",
                newName: "Stress");
        }
    }
}
