using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AprenderHolandes.Migrations
{
    public partial class Privatekeysamceevaluacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluaciones_MateriaCursadaEvaluacionMateriaCursadaId_MateriaCursadaEval~",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfesorId",
                table: "Evaluaciones",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "MateriaCursadaEvaluacionMateriaCursadaId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "MateriaCursadaEvaluacionEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "MateriaCursadaEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluaciones_MateriaCursadaEvaluacionMateriaCursadaId_MateriaCursadaEval~",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                columns: new[] { "MateriaCursadaEvaluacionMateriaCursadaId", "MateriaCursadaEvaluacionEvaluacionId" },
                principalTable: "MateriaCursadaEvaluaciones",
                principalColumns: new[] { "MateriaCursadaId", "EvaluacionId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluaciones_MateriaCursadaEvaluacionMateriaCursadaId_MateriaCursadaEval~",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas");

            migrationBuilder.DropColumn(
                name: "ProfesorId",
                table: "Evaluaciones");

            migrationBuilder.DropColumn(
                name: "MateriaCursadaEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas");

            migrationBuilder.AlterColumn<Guid>(
                name: "MateriaCursadaEvaluacionMateriaCursadaId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MateriaCursadaEvaluacionEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluaciones_MateriaCursadaEvaluacionMateriaCursadaId_MateriaCursadaEval~",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                columns: new[] { "MateriaCursadaEvaluacionMateriaCursadaId", "MateriaCursadaEvaluacionEvaluacionId" },
                principalTable: "MateriaCursadaEvaluaciones",
                principalColumns: new[] { "MateriaCursadaId", "EvaluacionId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
