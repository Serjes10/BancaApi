﻿// <auto-generated />
using System;
using BancaApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BancaApi.Migrations
{
    [DbContext(typeof(BancaDbContext))]
    [Migration("20250128220600_ActualizacionDominios")]
    partial class ActualizacionDominios
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("BancaApi.Domain.Entities.Cliente", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("estado")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("fechaCreacion")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("fechaNacimiento")
                        .HasColumnType("TEXT");

                    b.Property<string>("identificacion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ingresos")
                        .HasColumnType("TEXT");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("sexo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("BancaApi.Domain.Entities.ContadorCuentas", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("contador")
                        .HasColumnType("INTEGER");

                    b.Property<string>("correlativo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("tipoCuenta")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("ContadorCuentas");
                });

            modelBuilder.Entity("BancaApi.Domain.Entities.Cuenta", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("estado")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("fechaCreacion")
                        .HasColumnType("TEXT");

                    b.Property<int>("idCliente")
                        .HasColumnType("INTEGER");

                    b.Property<string>("numeroCuenta")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("saldo")
                        .HasColumnType("TEXT");

                    b.Property<string>("tipoCuenta")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("idCliente");

                    b.HasIndex("numeroCuenta")
                        .IsUnique();

                    b.ToTable("Cuentas");
                });

            modelBuilder.Entity("BancaApi.Domain.Entities.Transaccion", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("fecha")
                        .HasColumnType("TEXT");

                    b.Property<int>("idCuenta")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("monto")
                        .HasColumnType("TEXT");

                    b.Property<string>("tipoTransaccion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("idCuenta");

                    b.ToTable("Transacciones");
                });

            modelBuilder.Entity("BancaApi.Domain.Entities.Cuenta", b =>
                {
                    b.HasOne("BancaApi.Domain.Entities.Cliente", "cliente")
                        .WithMany()
                        .HasForeignKey("idCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("cliente");
                });

            modelBuilder.Entity("BancaApi.Domain.Entities.Transaccion", b =>
                {
                    b.HasOne("BancaApi.Domain.Entities.Cuenta", "cuenta")
                        .WithMany()
                        .HasForeignKey("idCuenta")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("cuenta");
                });
#pragma warning restore 612, 618
        }
    }
}
