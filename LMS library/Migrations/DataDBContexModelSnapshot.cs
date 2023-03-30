﻿// <auto-generated />
using System;
using LMS_library.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LMS_library.Migrations
{
    [DbContext(typeof(DataDBContex))]
    partial class DataDBContexModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LMS_library.Data.Course", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("courseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("courseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("createDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("pendingMaterial")
                        .HasColumnType("int");

                    b.Property<DateTime>("submission")
                        .HasColumnType("datetime2");

                    b.Property<int?>("userId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("LMS_library.Data.CourseMaterial", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int?>("UserId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("courseId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("fileSize")
                        .HasColumnType("int");

                    b.Property<int>("fileStatus")
                        .HasColumnType("int");

                    b.Property<string>("materialPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("materialTypeID")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("resourceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("submission_date")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("UserId");

                    b.HasIndex("courseId");

                    b.HasIndex("materialTypeID");

                    b.HasIndex("resourceId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("LMS_library.Data.Exam", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("courseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("create_At")
                        .HasColumnType("datetime2");

                    b.Property<int>("examStatus")
                        .HasColumnType("int");

                    b.Property<int>("examType")
                        .HasColumnType("int");

                    b.Property<string>("fileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("filePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fileType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("teacherEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("time")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("LMS_library.Data.Lesson", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("materialId")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("topicId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("materialId")
                        .IsUnique();

                    b.HasIndex("topicId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("LMS_library.Data.MaterialType", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("MaterialTypes");
                });

            modelBuilder.Entity("LMS_library.Data.PrivateFiles", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("fileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("filePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fileSize")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fileType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updateAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("uploadAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.ToTable("PrivateFiles");
                });

            modelBuilder.Entity("LMS_library.Data.ResourceList", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("lessonId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("lessonId");

                    b.ToTable("ResourceLists");
                });

            modelBuilder.Entity("LMS_library.Data.Role", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime>("create_At")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("update_At")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("LMS_library.Data.SystemDetail", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("libraryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lybraryEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("lybraryPhone")
                        .HasColumnType("int");

                    b.Property<string>("lybraryWebSite")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("principal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("schoolName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("schoolWebSite")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("shoolType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("System");
                });

            modelBuilder.Entity("LMS_library.Data.Topic", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("courseId")
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("courseId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("LMS_library.Data.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("passwordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("passwordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("resetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("resetTokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<int?>("roleId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("userCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("roleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LMS_library.Data.Course", b =>
                {
                    b.HasOne("LMS_library.Data.User", "User")
                        .WithMany("Course")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LMS_library.Data.CourseMaterial", b =>
                {
                    b.HasOne("LMS_library.Data.User", "User")
                        .WithMany("materials")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMS_library.Data.Course", "courses")
                        .WithMany("materials")
                        .HasForeignKey("courseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMS_library.Data.MaterialType", "MaterialType")
                        .WithMany("CourseMaterial")
                        .HasForeignKey("materialTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMS_library.Data.ResourceList", "ResourceList")
                        .WithMany("CourseMaterial")
                        .HasForeignKey("resourceId");

                    b.Navigation("MaterialType");

                    b.Navigation("ResourceList");

                    b.Navigation("User");

                    b.Navigation("courses");
                });

            modelBuilder.Entity("LMS_library.Data.Lesson", b =>
                {
                    b.HasOne("LMS_library.Data.CourseMaterial", "Material")
                        .WithOne("Lesson")
                        .HasForeignKey("LMS_library.Data.Lesson", "materialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMS_library.Data.Topic", "topic")
                        .WithMany("Lessons")
                        .HasForeignKey("topicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("topic");
                });

            modelBuilder.Entity("LMS_library.Data.PrivateFiles", b =>
                {
                    b.HasOne("LMS_library.Data.User", "User")
                        .WithMany("files")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LMS_library.Data.ResourceList", b =>
                {
                    b.HasOne("LMS_library.Data.Lesson", "Lesson")
                        .WithMany("resourceLists")
                        .HasForeignKey("lessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");
                });

            modelBuilder.Entity("LMS_library.Data.Topic", b =>
                {
                    b.HasOne("LMS_library.Data.Course", "courses")
                        .WithMany("topics")
                        .HasForeignKey("courseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("courses");
                });

            modelBuilder.Entity("LMS_library.Data.User", b =>
                {
                    b.HasOne("LMS_library.Data.Role", "Role")
                        .WithMany("users")
                        .HasForeignKey("roleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("LMS_library.Data.Course", b =>
                {
                    b.Navigation("materials");

                    b.Navigation("topics");
                });

            modelBuilder.Entity("LMS_library.Data.CourseMaterial", b =>
                {
                    b.Navigation("Lesson")
                        .IsRequired();
                });

            modelBuilder.Entity("LMS_library.Data.Lesson", b =>
                {
                    b.Navigation("resourceLists");
                });

            modelBuilder.Entity("LMS_library.Data.MaterialType", b =>
                {
                    b.Navigation("CourseMaterial");
                });

            modelBuilder.Entity("LMS_library.Data.ResourceList", b =>
                {
                    b.Navigation("CourseMaterial");
                });

            modelBuilder.Entity("LMS_library.Data.Role", b =>
                {
                    b.Navigation("users");
                });

            modelBuilder.Entity("LMS_library.Data.Topic", b =>
                {
                    b.Navigation("Lessons");
                });

            modelBuilder.Entity("LMS_library.Data.User", b =>
                {
                    b.Navigation("Course");

                    b.Navigation("files");

                    b.Navigation("materials");
                });
#pragma warning restore 612, 618
        }
    }
}
