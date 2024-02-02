using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class FileUploadsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "File",
                schema: "bk",
                table: "Books",
                newName: "FileId");

            migrationBuilder.AlterColumn<decimal>(
                name: "RatersTotal",
                schema: "bk",
                table: "Books",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RatersCount",
                schema: "bk",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                schema: "bk",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 2, 3, 38, 29, 496, DateTimeKind.Local).AddTicks(8564),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 1, 31, 22, 18, 23, 762, DateTimeKind.Local).AddTicks(2064));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReadAt",
                schema: "bk",
                table: "BookReaders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 2, 2, 3, 38, 29, 497, DateTimeKind.Local).AddTicks(4461),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 1, 31, 22, 18, 23, 762, DateTimeKind.Local).AddTicks(7646));

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoredFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bookId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadedFiles_Books_bookId",
                        column: x => x.bookId,
                        principalSchema: "bk",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFiles_bookId",
                table: "UploadedFiles",
                column: "bookId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadedFiles");

            migrationBuilder.RenameColumn(
                name: "FileId",
                schema: "bk",
                table: "Books",
                newName: "File");

            migrationBuilder.AlterColumn<decimal>(
                name: "RatersTotal",
                schema: "bk",
                table: "Books",
                type: "decimal(18,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<int>(
                name: "RatersCount",
                schema: "bk",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                schema: "bk",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 1, 31, 22, 18, 23, 762, DateTimeKind.Local).AddTicks(2064),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 2, 2, 3, 38, 29, 496, DateTimeKind.Local).AddTicks(8564));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReadAt",
                schema: "bk",
                table: "BookReaders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 1, 31, 22, 18, 23, 762, DateTimeKind.Local).AddTicks(7646),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 2, 2, 3, 38, 29, 497, DateTimeKind.Local).AddTicks(4461));
        }
    }
}
