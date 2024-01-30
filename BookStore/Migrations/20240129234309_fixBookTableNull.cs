using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class fixBookTableNull : Migration
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
                defaultValue: new DateTime(2024, 1, 30, 1, 43, 8, 982, DateTimeKind.Local).AddTicks(7932),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 1, 23, 19, 22, 48, 988, DateTimeKind.Local).AddTicks(1388));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                schema: "bk",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 1, 23, 19, 22, 48, 988, DateTimeKind.Local).AddTicks(1388),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 1, 30, 1, 43, 8, 982, DateTimeKind.Local).AddTicks(7932));
        }
    }
}
