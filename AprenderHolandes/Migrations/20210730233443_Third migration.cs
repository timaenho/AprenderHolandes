using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AprenderHolandes.Migrations
{
    public partial class Thirdmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MateriaCursadaId = table.Column<Guid>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Descripcion = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: false),
                    ProfesorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clases_MateriaCursadas_MateriaCursadaId",
                        column: x => x.MateriaCursadaId,
                        principalTable: "MateriaCursadas",
                        principalColumn: "MateriaCursadaId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Clases_Personas_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Evaluaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Titulo = table.Column<string>(maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(nullable: false),
                    MateriaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evaluaciones_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "MateriaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MateriaCursadaEvaluaciones",
                columns: table => new
                {
                    EvaluacionId = table.Column<Guid>(nullable: false),
                    MateriaCursadaId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    Activo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriaCursadaEvaluaciones", x => new { x.MateriaCursadaId, x.EvaluacionId });
                    table.ForeignKey(
                        name: "FK_MateriaCursadaEvaluaciones_Evaluaciones_EvaluacionId",
                        column: x => x.EvaluacionId,
                        principalTable: "Evaluaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MateriaCursadaEvaluaciones_MateriaCursadas_MateriaCursadaId",
                        column: x => x.MateriaCursadaId,
                        principalTable: "MateriaCursadas",
                        principalColumn: "MateriaCursadaId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AlumnoMateriaCursadaEvaluaciondaNotas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AlumnoId = table.Column<Guid>(nullable: false),
                    MateriaCursadaEvaluacionMateriaCursadaId = table.Column<Guid>(nullable: false),
                    MateriaCursadaEvaluacionEvaluacionId = table.Column<Guid>(nullable: false),
                    Nota = table.Column<string>(nullable: false),
                    ProfesorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlumnoMateriaCursadaEvaluaciondaNotas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlumnoMateriaCursadaEvaluaciondaNotas_Personas_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AlumnoMateriaCursadaEvaluaciondaNotas_Personas_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluaciones_MateriaCursadaEvaluacionMateriaCursadaId_MateriaCursadaEval~",
                        columns: x => new { x.MateriaCursadaEvaluacionMateriaCursadaId, x.MateriaCursadaEvaluacionEvaluacionId },
                        principalTable: "MateriaCursadaEvaluaciones",
                        principalColumns: new[] { "MateriaCursadaId", "EvaluacionId" },
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoMateriaCursadaEvaluaciondaNotas_AlumnoId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoMateriaCursadaEvaluaciondaNotas_ProfesorId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoMateriaCursadaEvaluaciondaNotas_MateriaCursadaEvaluacionMateriaCursadaId_MateriaCursadaEvaluacionEvaluacionId",
                table: "AlumnoMateriaCursadaEvaluaciondaNotas",
                columns: new[] { "MateriaCursadaEvaluacionMateriaCursadaId", "MateriaCursadaEvaluacionEvaluacionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Clases_MateriaCursadaId",
                table: "Clases",
                column: "MateriaCursadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Clases_ProfesorId",
                table: "Clases",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluaciones_MateriaId",
                table: "Evaluaciones",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaCursadaEvaluaciones_EvaluacionId",
                table: "MateriaCursadaEvaluaciones",
                column: "EvaluacionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlumnoMateriaCursadaEvaluaciondaNotas");

            migrationBuilder.DropTable(
                name: "Clases");

            migrationBuilder.DropTable(
                name: "MateriaCursadaEvaluaciones");

            migrationBuilder.DropTable(
                name: "Evaluaciones");
        }
    }
}
