using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class AddQuizResultToAnswerUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizResultId",
                table: "AnswerUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUsers_QuizResultId",
                table: "AnswerUsers",
                column: "QuizResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerUsers_QuizResults_QuizResultId",
                table: "AnswerUsers",
                column: "QuizResultId",
                principalTable: "QuizResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerUsers_QuizResults_QuizResultId",
                table: "AnswerUsers");

            migrationBuilder.DropIndex(
                name: "IX_AnswerUsers_QuizResultId",
                table: "AnswerUsers");

            migrationBuilder.DropColumn(
                name: "QuizResultId",
                table: "AnswerUsers");
        }
    }
}
