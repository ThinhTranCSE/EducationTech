using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class AddTopicIdFieldsToLearningObjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "LearningObjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LearningObjects_TopicId",
                table: "LearningObjects",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningObjects_RecommendTopics_TopicId",
                table: "LearningObjects",
                column: "TopicId",
                principalTable: "RecommendTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningObjects_RecommendTopics_TopicId",
                table: "LearningObjects");

            migrationBuilder.DropIndex(
                name: "IX_LearningObjects_TopicId",
                table: "LearningObjects");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "LearningObjects");
        }
    }
}
