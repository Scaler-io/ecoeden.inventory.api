using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecoeden.Inventory.Infrastructure.Database.SQL.Migrations.StockDbContext
{
    /// <inheritdoc />
    public partial class AddsColumnCreatedAndUpdatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Quantity",
                schema: "ecoeden.stock",
                table: "ProductStocks",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ecoeden.stock",
                table: "ProductStocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "ecoeden.stock",
                table: "ProductStocks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ecoeden.stock",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "ecoeden.stock",
                table: "ProductStocks");

            migrationBuilder.AlterColumn<string>(
                name: "Quantity",
                schema: "ecoeden.stock",
                table: "ProductStocks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
