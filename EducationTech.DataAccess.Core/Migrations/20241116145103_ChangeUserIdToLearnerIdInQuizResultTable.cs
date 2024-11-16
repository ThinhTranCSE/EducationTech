using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class ChangeUserIdToLearnerIdInQuizResultTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizResults_Users_UserId",
                table: "QuizResults");

            migrationBuilder.DropIndex(
                name: "IX_QuizResults_UserId",
                table: "QuizResults");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "QuizResults");

            migrationBuilder.AddColumn<int>(
                name: "LearnerId",
                table: "QuizResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QuizResults_LearnerId",
                table: "QuizResults",
                column: "LearnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResults_Learners_LearnerId",
                table: "QuizResults",
                column: "LearnerId",
                principalTable: "Learners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizResults_Learners_LearnerId",
                table: "QuizResults");

            migrationBuilder.DropIndex(
                name: "IX_QuizResults_LearnerId",
                table: "QuizResults");

            migrationBuilder.DropColumn(
                name: "LearnerId",
                table: "QuizResults");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "QuizResults",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_QuizResults_UserId",
                table: "QuizResults",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResults_Users_UserId",
                table: "QuizResults",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
