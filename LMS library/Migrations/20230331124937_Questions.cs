using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_library.Migrations
{
    /// <inheritdoc />
    public partial class Questions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    difficultLevel = table.Column<int>(type: "int", nullable: false),
                    courseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    teacherEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    questionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answerA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answerB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answerC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answerD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    correctAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    create_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
