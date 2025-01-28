using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancaApi.Migrations
{
    /// <inheritdoc />
    public partial class AddContadorCuentas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tipoCuenta",
                table: "Cuentas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_numeroCuenta",
                table: "Cuentas",
                column: "numeroCuenta",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cuentas_numeroCuenta",
                table: "Cuentas");

            migrationBuilder.DropColumn(
                name: "tipoCuenta",
                table: "Cuentas");
        }
    }
}
