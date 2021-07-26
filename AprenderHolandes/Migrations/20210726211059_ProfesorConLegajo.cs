using Microsoft.EntityFrameworkCore.Migrations;

namespace AprenderHolandes.Migrations
{
    public partial class ProfesorConLegajo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Profesor_Legajo",
                table: "Personas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profesor_Legajo",
                table: "Personas");
        }
    }
}
