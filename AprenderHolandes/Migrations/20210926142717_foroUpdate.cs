using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AprenderHolandes.Migrations
{
    public partial class foroUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PreguntaId",
                table: "Mensajes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_PreguntaId",
                table: "Mensajes",
                column: "PreguntaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensajes_Mensajes_PreguntaId",
                table: "Mensajes",
                column: "PreguntaId",
                principalTable: "Mensajes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mensajes_Mensajes_PreguntaId",
                table: "Mensajes");

            migrationBuilder.DropIndex(
                name: "IX_Mensajes_PreguntaId",
                table: "Mensajes");

            migrationBuilder.DropColumn(
                name: "PreguntaId",
                table: "Mensajes");
        }
    }
}
