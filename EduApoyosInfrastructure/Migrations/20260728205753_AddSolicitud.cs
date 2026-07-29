using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduApoyosInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSolicitud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolicitudesApoyo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstudianteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoApoyo = table.Column<int>(type: "int", nullable: false),
                    MontoSolicitado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AsesorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesApoyo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudesApoyo_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudesApoyo_Usuarios_AsesorId",
                        column: x => x.AsesorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistorialEstados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SolicitudId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstadoAnterior = table.Column<int>(type: "int", nullable: false),
                    EstadoNuevo = table.Column<int>(type: "int", nullable: false),
                    FechaCambio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialEstados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialEstados_SolicitudesApoyo_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "SolicitudesApoyo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialEstados_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstados_SolicitudId",
                table: "HistorialEstados",
                column: "SolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstados_UsuarioId",
                table: "HistorialEstados",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesApoyo_AsesorId",
                table: "SolicitudesApoyo",
                column: "AsesorId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesApoyo_EstudianteId",
                table: "SolicitudesApoyo",
                column: "EstudianteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialEstados");

            migrationBuilder.DropTable(
                name: "SolicitudesApoyo");
        }
    }
}
