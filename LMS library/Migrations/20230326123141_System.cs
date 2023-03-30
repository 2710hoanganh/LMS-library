using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_library.Migrations
{
    /// <inheritdoc />
    public partial class System : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    lybraryEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_System", x => x.id);
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
                    passwordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    resetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    resetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
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
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    topicId = table.Column<int>(type: "int", nullable: false),
                    materialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.id);
                    table.ForeignKey(
                        name: "FK_Lessons_Topics_topicId",
                        column: x => x.topicId,
                        principalTable: "Topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceLists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lessonId = table.Column<int>(type: "int", nullable: false)
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
                    resourceId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_Materials_ResourceLists_resourceId",
                        column: x => x.resourceId,
                        principalTable: "ResourceLists",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Materials_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

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
                name: "IX_Materials_resourceId",
                table: "Materials",
                column: "resourceId");

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
                name: "IX_Topics_courseId",
                table: "Topics",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_roleId",
                table: "Users",
                column: "roleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Materials_materialId",
                table: "Lessons",
                column: "materialId",
                principalTable: "Materials",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Users_userId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Users_UserId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Materials_materialId",
                table: "Lessons");

            migrationBuilder.DropTable(
                name: "PrivateFiles");

            migrationBuilder.DropTable(
                name: "System");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "MaterialTypes");

            migrationBuilder.DropTable(
                name: "ResourceLists");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
