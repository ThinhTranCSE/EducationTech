using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class UpdateNewFieldsForCourseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "RecommendTopics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "RecommendTopics");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Courses",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
