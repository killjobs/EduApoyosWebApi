using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduApoyosInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioTokens_Usuarios_UsuarioId",
                table: "UsuarioTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioTokens",
                table: "UsuarioTokens");

            migrationBuilder.RenameTable(
                name: "UsuarioTokens",
                newName: "UsuariosToken");

            migrationBuilder.RenameIndex(
                name: "IX_UsuarioTokens_UsuarioId",
                table: "UsuariosToken",
                newName: "IX_UsuariosToken_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuariosToken",
                table: "UsuariosToken",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosToken_Usuarios_UsuarioId",
                table: "UsuariosToken",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosToken_Usuarios_UsuarioId",
                table: "UsuariosToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuariosToken",
                table: "UsuariosToken");

            migrationBuilder.RenameTable(
                name: "UsuariosToken",
                newName: "UsuarioTokens");

            migrationBuilder.RenameIndex(
                name: "IX_UsuariosToken_UsuarioId",
                table: "UsuarioTokens",
                newName: "IX_UsuarioTokens_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioTokens",
                table: "UsuarioTokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioTokens_Usuarios_UsuarioId",
                table: "UsuarioTokens",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
