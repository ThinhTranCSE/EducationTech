using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class AddMoreFiledsToLearnerLogsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attempt",
                table: "LearnerLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "LearnerLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeTaken",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attempt",
                table: "LearnerLogs");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "LearnerLogs");

            migrationBuilder.DropColumn(
                name: "TimeTaken",
                table: "LearnerLogs");

            migrationBuilder.DropColumn(
                name: "VisitedAt",
                table: "LearnerLogs");

            migrationBuilder.DropColumn(
                name: "VisitedTime",
                table: "LearnerLogs");
        }
    }
}
