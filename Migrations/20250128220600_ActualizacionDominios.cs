using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancaApi.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionDominios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContadorCuentas",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    contador = table.Column<int>(type: "INTEGER", nullable: false),
                    tipoCuenta = table.Column<string>(type: "TEXT", nullable: false),
                    correlativo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContadorCuentas", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContadorCuentas");
        }
    }
}
