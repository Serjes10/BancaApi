using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancaApi.Migrations
{
    /// <inheritdoc />
    public partial class BancaApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    identificacion = table.Column<string>(type: "TEXT", nullable: false),
                    fechaNacimiento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    sexo = table.Column<string>(type: "TEXT", nullable: false),
                    ingresos = table.Column<decimal>(type: "TEXT", nullable: false),
                    fechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    estado = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cuentas",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    numeroCuenta = table.Column<string>(type: "TEXT", nullable: false),
                    saldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    idCliente = table.Column<int>(type: "INTEGER", nullable: false),
                    fechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    estado = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuentas", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cuentas_Clientes_idCliente",
                        column: x => x.idCliente,
                        principalTable: "Clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tipoTransaccion = table.Column<string>(type: "TEXT", nullable: false),
                    monto = table.Column<decimal>(type: "TEXT", nullable: false),
                    fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    idCuenta = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transacciones_Cuentas_idCuenta",
                        column: x => x.idCuenta,
                        principalTable: "Cuentas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_idCliente",
                table: "Cuentas",
                column: "idCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_idCuenta",
                table: "Transacciones",
                column: "idCuenta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "Cuentas");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
