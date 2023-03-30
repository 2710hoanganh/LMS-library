using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_library.Migrations
{
    /// <inheritdoc />
    public partial class Exams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    filePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    courseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    teacherEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    examType = table.Column<int>(type: "int", nullable: false),
                    time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    examStatus = table.Column<int>(type: "int", nullable: false),
                    create_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exams");
        }
    }
}
