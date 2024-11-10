using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class ReomoveBranhIdFromCoursesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Branches_BranchId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_BranchId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Courses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_BranchId",
                table: "Courses",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Branches_BranchId",
                table: "Courses",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
