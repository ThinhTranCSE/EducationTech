using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class RemoveUnuseFieldFromLogAndAddSemesterFieldToSavingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "LearnerLogs");

            migrationBuilder.DropColumn(
                name: "VisitedAt",
                table: "LearnerLogs");

            migrationBuilder.DropColumn(
                name: "VisitedTime",
                table: "LearnerLogs");

            migrationBuilder.AddColumn<int>(
                name: "Semester",
                table: "CourseLearningPathOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Semester",
                table: "CourseLearningPathOrders");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "LearnerLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "VisitedAt",
                table: "LearnerLogs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "VisitedTime",
                table: "LearnerLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
