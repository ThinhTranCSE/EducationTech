using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class RemoveCourseSectionAndLessonTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Lessons_LessonId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Lessons_LessonId",
                table: "Videos");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "CourseSections");

            migrationBuilder.DropIndex(
                name: "IX_Videos_LessonId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_LessonId",
                table: "Quizzes");

            // Xóa cột cũ "LessonId" và tạo lại cột mới "LearningObjectId"
            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Quizzes");

            migrationBuilder.AddColumn<int>(
                name: "LearningObjectId",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LearningObjectId",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_LearningObjectId",
                table: "Videos",
                column: "LearningObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_LearningObjectId",
                table: "Quizzes",
                column: "LearningObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_LearningObjects_LearningObjectId",
                table: "Quizzes",
                column: "LearningObjectId",
                principalTable: "LearningObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_LearningObjects_LearningObjectId",
                table: "Videos",
                column: "LearningObjectId",
                principalTable: "LearningObjects",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_LearningObjects_LearningObjectId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_LearningObjects_LearningObjectId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_LearningObjectId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_LearningObjectId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "LearningObjectId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LearningObjectId",
                table: "Quizzes");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CourseSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseSections_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CourseSectionId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_CourseSections_CourseSectionId",
                        column: x => x.CourseSectionId,
                        principalTable: "CourseSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_LessonId",
                table: "Videos",
                column: "LessonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_LessonId",
                table: "Quizzes",
                column: "LessonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseSections_CourseId",
                table: "CourseSections",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CourseSectionId",
                table: "Lessons",
                column: "CourseSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Lessons_LessonId",
                table: "Quizzes",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Lessons_LessonId",
                table: "Videos",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }
    }
}
