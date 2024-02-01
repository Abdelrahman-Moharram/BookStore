using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class BookTableRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                schema: "bk",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 1, 31, 21, 54, 10, 876, DateTimeKind.Local).AddTicks(7105),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 1, 30, 1, 43, 8, 982, DateTimeKind.Local).AddTicks(7932));

            migrationBuilder.AddColumn<int>(
                name: "RatersCount",
                schema: "bk",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RatersTotal",
                schema: "bk",
                table: "Books",
                type: "decimal(18,6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatersCount",
                schema: "bk",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RatersTotal",
                schema: "bk",
                table: "Books");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                schema: "bk",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 1, 30, 1, 43, 8, 982, DateTimeKind.Local).AddTicks(7932),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 1, 31, 21, 54, 10, 876, DateTimeKind.Local).AddTicks(7105));
        }
    }
}
