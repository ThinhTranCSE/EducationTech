using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class ChangeLearningObjectToNullableFromVideoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<int>(
                name: "LearningObjectId",
                table: "Videos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
