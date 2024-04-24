using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class ChangeLessonIdInVideosTableToNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Lessons_LessonId",
                table: "Videos");

            migrationBuilder.AlterColumn<int>(
                name: "LessonId",
                table: "Videos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Lessons_LessonId",
                table: "Videos",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Lessons_LessonId",
                table: "Videos");

            migrationBuilder.AlterColumn<int>(
                name: "LessonId",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Lessons_LessonId",
                table: "Videos",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
