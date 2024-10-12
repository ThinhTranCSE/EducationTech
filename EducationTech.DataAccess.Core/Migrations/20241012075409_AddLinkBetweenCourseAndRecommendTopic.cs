using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class AddLinkBetweenCourseAndRecommendTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "RecommendTopics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecommendTopics_CourseId",
                table: "RecommendTopics",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendTopics_Courses_CourseId",
                table: "RecommendTopics",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendTopics_Courses_CourseId",
                table: "RecommendTopics");

            migrationBuilder.DropIndex(
                name: "IX_RecommendTopics_CourseId",
                table: "RecommendTopics");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "RecommendTopics");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Courses",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
