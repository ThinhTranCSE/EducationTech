using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class AddForgeinKeyInCommentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscussionId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DiscussionId",
                table: "Comments",
                column: "DiscussionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Discussions_DiscussionId",
                table: "Comments",
                column: "DiscussionId",
                principalTable: "Discussions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Discussions_DiscussionId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_DiscussionId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DiscussionId",
                table: "Comments");
        }
    }
}
