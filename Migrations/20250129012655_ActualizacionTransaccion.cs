using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancaApi.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionTransaccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "estado",
                table: "Transacciones",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "saldoFinal",
                table: "Transacciones",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estado",
                table: "Transacciones");

            migrationBuilder.DropColumn(
                name: "saldoFinal",
                table: "Transacciones");
        }
    }
}
