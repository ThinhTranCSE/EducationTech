using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class AndNestedSetColumnsToCategoriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Left",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Right",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TreeId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Left",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Right",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TreeId",
                table: "Categories");
        }
    }
}
