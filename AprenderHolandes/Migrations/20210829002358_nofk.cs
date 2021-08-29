using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AprenderHolandes.Migrations
{
    public partial class nofk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluaciones_MateriaCursadaEvaluacionMateriaCursadaId_MateriaCursadaEval~",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MateriaCursadaEvaluaciones",
                table: "MateriaCursadaEvaluaciones");

            migrationBuilder.DropIndex(
                name: "IX_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluacionMateriaCursadaId_MateriaCursadaEvaluacionEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas");

            migrationBuilder.DropColumn(
                name: "MateriaCursadaEvaluacionEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas");

            migrationBuilder.DropColumn(
                name: "MateriaCursadaEvaluacionMateriaCursadaId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "MateriaCursadaEvaluaciones",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MateriaCursadaEvaluaciones",
                table: "MateriaCursadaEvaluaciones",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaCursadaEvaluaciones_MateriaCursadaId",
                table: "MateriaCursadaEvaluaciones",
                column: "MateriaCursadaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                column: "MateriaCursadaEvaluacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluaciones_MateriaCursadaEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                column: "MateriaCursadaEvaluacionId",
                principalTable: "MateriaCursadaEvaluaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluaciones_MateriaCursadaEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MateriaCursadaEvaluaciones",
                table: "MateriaCursadaEvaluaciones");

            migrationBuilder.DropIndex(
                name: "IX_MateriaCursadaEvaluaciones_MateriaCursadaId",
                table: "MateriaCursadaEvaluaciones");

            migrationBuilder.DropIndex(
                name: "IX_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MateriaCursadaEvaluaciones");

            migrationBuilder.AddColumn<Guid>(
                name: "MateriaCursadaEvaluacionEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MateriaCursadaEvaluacionMateriaCursadaId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MateriaCursadaEvaluaciones",
                table: "MateriaCursadaEvaluaciones",
                columns: new[] { "MateriaCursadaId", "EvaluacionId" });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluacionMateriaCursadaId_MateriaCursadaEvaluacionEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                columns: new[] { "MateriaCursadaEvaluacionMateriaCursadaId", "MateriaCursadaEvaluacionEvaluacionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluaciones_MateriaCursadaEvaluacionMateriaCursadaId_MateriaCursadaEval~",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                columns: new[] { "MateriaCursadaEvaluacionMateriaCursadaId", "MateriaCursadaEvaluacionEvaluacionId" },
                principalTable: "MateriaCursadaEvaluaciones",
                principalColumns: new[] { "MateriaCursadaId", "EvaluacionId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
