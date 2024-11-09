using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class ChangeSomeLeanerFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Learners");

            migrationBuilder.DropColumn(
                name: "BackgroundKnowledge",
                table: "Learners");

            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Learners");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Learners");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Learners");

            migrationBuilder.DropColumn(
                name: "Qualification",
                table: "Learners");

            migrationBuilder.AddColumn<int>(
                name: "SpecialityId",
                table: "Learners",
                type: "int",
                nullable: false,
                defaultValue: 0
                );

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Learners",
                type: "char(36)",
                nullable: false
                );


            migrationBuilder.CreateIndex(
                name: "IX_Learners_SpecialityId",
                table: "Learners",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_Learners_UserId",
                table: "Learners",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Learners_Specialities_SpecialityId",
                table: "Learners",
                column: "SpecialityId",
                principalTable: "Specialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Learners_Users_UserId",
                table: "Learners",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Learners_Specialities_SpecialityId",
                table: "Learners");

            migrationBuilder.DropForeignKey(
                name: "FK_Learners_Users_UserId",
                table: "Learners");

            migrationBuilder.DropIndex(
                name: "IX_Learners_SpecialityId",
                table: "Learners");

            migrationBuilder.DropIndex(
                name: "IX_Learners_UserId",
                table: "Learners");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Learners");

            migrationBuilder.DropColumn(
                name: "SpecialityId",
                table: "Learners"
                );

            migrationBuilder.AddColumn<string>(
                name: "Qualification",
                table: "Learners",
                type: "longtext",
                nullable: false
                );

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Learners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BackgroundKnowledge",
                table: "Learners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "Learners",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Learners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Learners",
                type: "longtext",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_LearningStyles_LearnerId",
                table: "LearningStyles",
                column: "LearnerId",
                unique: true);
        }
    }
}
