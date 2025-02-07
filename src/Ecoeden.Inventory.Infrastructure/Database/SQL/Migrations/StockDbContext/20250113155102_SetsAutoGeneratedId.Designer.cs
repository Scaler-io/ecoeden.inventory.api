﻿// <auto-generated />
using System;
using Ecoeden.Inventory.Infrastructure.Database.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ecoeden.Inventory.Infrastructure.Database.SQL.Migrations.StockDbContext
{
    [DbContext(typeof(EcoedenStockDbContext))]
    [Migration("20250113155102_SetsAutoGeneratedId")]
    partial class SetsAutoGeneratedId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("ecoeden.stock")
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ecoeden.Inventory.Domain.Entities.SQL.ProductStock", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SupplierId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CorrelationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId", "SupplierId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("ProductId", "SupplierId"));

                    b.ToTable("ProductStocks", "ecoeden.stock");
                });
#pragma warning restore 612, 618
        }
    }
}
