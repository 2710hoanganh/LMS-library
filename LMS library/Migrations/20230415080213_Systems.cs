using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_library.Migrations
{
    /// <inheritdoc />
    public partial class Systems : Migration
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

            migrationBuilder.CreateTable(
                name: "MaterialTypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    isRead = table.Column<bool>(type: "bit", nullable: false),
                    create_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    create_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SendHelps",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendHelps", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "System",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    schoolName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    schoolWebSite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shoolType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    principal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    libraryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lybraryWebSite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lybraryPhone = table.Column<int>(type: "int", nullable: false),
                    lybraryEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_System", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    classCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    className = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    teacherEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    courseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex = table.Column<int>(type: "int", nullable: false),
                    phone = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    classId = table.Column<int>(type: "int", nullable: true),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    resetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    resetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Classes_classId",
                        column: x => x.classId,
                        principalTable: "Classes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Users_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    courseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    submission = table.Column<DateTime>(type: "datetime2", nullable: false),
                    pendingMaterial = table.Column<int>(type: "int", nullable: false),
                    createDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Courses_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivateFiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    filePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    uploadAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateFiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_PrivateFiles_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    materialTypeID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    courseId = table.Column<int>(type: "int", nullable: false),
                    fileStatus = table.Column<int>(type: "int", nullable: false),
                    materialPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileSize = table.Column<int>(type: "int", nullable: false),
                    submission_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.id);
                    table.ForeignKey(
                        name: "FK_Materials_Courses_courseId",
                        column: x => x.courseId,
                        principalTable: "Courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materials_MaterialTypes_materialTypeID",
                        column: x => x.materialTypeID,
                        principalTable: "MaterialTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materials_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    courseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.id);
                    table.ForeignKey(
                        name: "FK_Topics_Courses_courseId",
                        column: x => x.courseId,
                        principalTable: "Courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    topicId = table.Column<int>(type: "int", nullable: false),
                    materialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.id);
                    table.ForeignKey(
                        name: "FK_Lessons_Materials_materialId",
                        column: x => x.materialId,
                        principalTable: "Materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Topics_topicId",
                        column: x => x.topicId,
                        principalTable: "Topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ResourceLists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lessonId = table.Column<int>(type: "int", nullable: false),
                    materialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceLists", x => x.id);
                    table.ForeignKey(
                        name: "FK_ResourceLists_Lessons_lessonId",
                        column: x => x.lessonId,
                        principalTable: "Lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceLists_Materials_materialId",
                        column: x => x.materialId,
                        principalTable: "Materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_courseId",
                table: "Classes",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_userId",
                table: "Courses",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_materialId",
                table: "Lessons",
                column: "materialId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_topicId",
                table: "Lessons",
                column: "topicId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_courseId",
                table: "Materials",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_materialTypeID",
                table: "Materials",
                column: "materialTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_UserId",
                table: "Materials",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateFiles_userId",
                table: "PrivateFiles",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceLists_lessonId",
                table: "ResourceLists",
                column: "lessonId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceLists_materialId",
                table: "ResourceLists",
                column: "materialId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_courseId",
                table: "Topics",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_classId",
                table: "Users",
                column: "classId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_roleId",
                table: "Users",
                column: "roleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Courses_courseId",
                table: "Classes",
                column: "courseId",
                principalTable: "Courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Courses_courseId",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PrivateFiles");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "ResourceLists");

            migrationBuilder.DropTable(
                name: "SendHelps");

            migrationBuilder.DropTable(
                name: "System");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "MaterialTypes");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
