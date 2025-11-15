using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KooliProjekt.Application.Migrations
{
    /// <inheritdoc />
    public partial class SeedOrdersAndOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "OrderDate", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 15, 13, 16, 41, 736, DateTimeKind.Local).AddTicks(4493), "Paid" },
                    { 2, new DateTime(2025, 11, 14, 13, 16, 41, 736, DateTimeKind.Local).AddTicks(4501), "Paid" },
                    { 3, new DateTime(2025, 11, 13, 13, 16, 41, 736, DateTimeKind.Local).AddTicks(4505), "Paid" },
                    { 4, new DateTime(2025, 11, 12, 13, 16, 41, 736, DateTimeKind.Local).AddTicks(4510), "Paid" },
                    { 5, new DateTime(2025, 11, 11, 13, 16, 41, 736, DateTimeKind.Local).AddTicks(4514), "Paid" }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "OrderId", "PriceAtOrder", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 699m, 1, 1 },
                    { 2, 1, 1200m, 2, 2 },
                    { 3, 2, 199m, 3, 1 },
                    { 4, 2, 25m, 4, 3 },
                    { 5, 3, 60m, 5, 1 },
                    { 6, 3, 120m, 6, 1 },
                    { 7, 4, 15m, 7, 2 },
                    { 8, 4, 30m, 8, 1 },
                    { 9, 5, 5m, 9, 4 },
                    { 10, 5, 350m, 10, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
