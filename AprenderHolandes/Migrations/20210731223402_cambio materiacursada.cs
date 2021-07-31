using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AprenderHolandes.Migrations
{
    public partial class cambiomateriacursada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clases_MateriaCursadas_MateriaCursadaId",
                table: "Clases");

            migrationBuilder.DropForeignKey(
                name: "FK_Clases_Personas_ProfesorId",
                table: "Clases");

            migrationBuilder.DropColumn(
                name: "Anio",
                table: "MateriaCursadas");

            migrationBuilder.DropColumn(
                name: "Cuatrimestre",
                table: "MateriaCursadas");

            migrationBuilder.AddColumn<int>(
                name: "CantidadHorasPorSemana",
                table: "MateriaCursadas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "MateriaCursadas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Dia",
                table: "MateriaCursadas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio",
                table: "MateriaCursadas",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaTermino",
                table: "MateriaCursadas",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Hora",
                table: "MateriaCursadas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Precio",
                table: "MateriaCursadas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_MateriaCursadas_MateriaCursadaId",
                table: "Clases",
                column: "MateriaCursadaId",
                principalTable: "MateriaCursadas",
                principalColumn: "MateriaCursadaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_Personas_ProfesorId",
                table: "Clases",
                column: "ProfesorId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clases_MateriaCursadas_MateriaCursadaId",
                table: "Clases");

            migrationBuilder.DropForeignKey(
                name: "FK_Clases_Personas_ProfesorId",
                table: "Clases");

            migrationBuilder.DropColumn(
                name: "CantidadHorasPorSemana",
                table: "MateriaCursadas");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "MateriaCursadas");

            migrationBuilder.DropColumn(
                name: "Dia",
                table: "MateriaCursadas");

            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "MateriaCursadas");

            migrationBuilder.DropColumn(
                name: "FechaTermino",
                table: "MateriaCursadas");

            migrationBuilder.DropColumn(
                name: "Hora",
                table: "MateriaCursadas");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "MateriaCursadas");

            migrationBuilder.AddColumn<int>(
                name: "Anio",
                table: "MateriaCursadas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Cuatrimestre",
                table: "MateriaCursadas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_MateriaCursadas_MateriaCursadaId",
                table: "Clases",
                column: "MateriaCursadaId",
                principalTable: "MateriaCursadas",
                principalColumn: "MateriaCursadaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_Personas_ProfesorId",
                table: "Clases",
                column: "ProfesorId",
                principalTable: "Personas",
                principalColumn: "Id");
        }
    }
}
