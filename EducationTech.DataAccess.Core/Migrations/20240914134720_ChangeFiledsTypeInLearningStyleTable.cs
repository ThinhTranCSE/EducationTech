using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class ChangeFiledsTypeInLearningStyleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningStyles_Learners_LearnerId",
                table: "LearningStyles");

            migrationBuilder.DropIndex(
                name: "IX_LearningStyles_LearnerId",
                table: "LearningStyles");

            migrationBuilder.AlterColumn<float>(
                name: "Visual",
                table: "LearningStyles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Verbal",
                table: "LearningStyles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Sequential",
                table: "LearningStyles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Sensing",
                table: "LearningStyles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Reflective",
                table: "LearningStyles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "LearnerId",
                table: "LearningStyles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Intuitive",
                table: "LearningStyles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Global",
                table: "LearningStyles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Active",
                table: "LearningStyles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Id",
                table: "LearningStyles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Visual",
                table: "LearningStyles",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Verbal",
                table: "LearningStyles",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Sequential",
                table: "LearningStyles",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Sensing",
                table: "LearningStyles",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Reflective",
                table: "LearningStyles",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "LearnerId",
                table: "LearningStyles",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Intuitive",
                table: "LearningStyles",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Global",
                table: "LearningStyles",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Active",
                table: "LearningStyles",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LearningStyles",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

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
    }
}
