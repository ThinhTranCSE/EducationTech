using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class KeyContrainForLearningStyleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Learners_LearningStyles_LearningStyleId",
                table: "Learners");

            migrationBuilder.DropIndex(
                name: "IX_Learners_LearningStyleId",
                table: "Learners");

            migrationBuilder.DropColumn(
                name: "LearningStyleId",
                table: "Learners");

            migrationBuilder.AlterColumn<int>(
                name: "LearnerId",
                table: "LearningStyles",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_LearningStyles_LearnerId",
                table: "LearningStyles",
                column: "LearnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LearningStyles_Learners_LearnerId",
                table: "LearningStyles",
                column: "LearnerId",
                principalTable: "Learners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningStyles_Learners_LearnerId",
                table: "LearningStyles");

            migrationBuilder.DropIndex(
                name: "IX_LearningStyles_LearnerId",
                table: "LearningStyles");

            migrationBuilder.AlterColumn<float>(
                name: "LearnerId",
                table: "LearningStyles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<float>(
                name: "LearningStyleId",
                table: "Learners",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_Learners_LearningStyleId",
                table: "Learners",
                column: "LearningStyleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Learners_LearningStyles_LearningStyleId",
                table: "Learners",
                column: "LearningStyleId",
                principalTable: "LearningStyles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
