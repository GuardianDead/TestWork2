using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestWork2.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "FileResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElapsedTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    MinDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AverageSeconds = table.Column<double>(type: "float", nullable: false),
                    MedianSeconds = table.Column<double>(type: "float", nullable: false),
                    AverageIndicator = table.Column<double>(type: "float", nullable: false),
                    MaxIndicator = table.Column<double>(type: "float", nullable: false),
                    MinIndicator = table.Column<double>(type: "float", nullable: false),
                    RowCount = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileResults_Files_FileName",
                        column: x => x.FileName,
                        principalTable: "Files",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Seconds = table.Column<int>(type: "int", nullable: false),
                    Indicator = table.Column<double>(type: "float", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileValues_Files_FileName",
                        column: x => x.FileName,
                        principalTable: "Files",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileResults_FileName",
                table: "FileResults",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_FileValues_FileName",
                table: "FileValues",
                column: "FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileResults");

            migrationBuilder.DropTable(
                name: "FileValues");

            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
