using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class RemoveFieldsFromLOTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AggregationLevel",
                table: "LearningObjects");

            migrationBuilder.DropColumn(
                name: "Format",
                table: "LearningObjects");

            migrationBuilder.DropColumn(
                name: "InteractivityLevel",
                table: "LearningObjects");

            migrationBuilder.DropColumn(
                name: "InteractivityType",
                table: "LearningObjects");

            migrationBuilder.DropColumn(
                name: "LearningResourceType",
                table: "LearningObjects");

            migrationBuilder.DropColumn(
                name: "SemanticDensity",
                table: "LearningObjects");

            migrationBuilder.DropColumn(
                name: "Structure",
                table: "LearningObjects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AggregationLevel",
                table: "LearningObjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Format",
                table: "LearningObjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InteractivityLevel",
                table: "LearningObjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InteractivityType",
                table: "LearningObjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LearningResourceType",
                table: "LearningObjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SemanticDensity",
                table: "LearningObjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Structure",
                table: "LearningObjects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
