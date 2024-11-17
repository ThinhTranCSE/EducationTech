using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class RemoveTopicConjunctionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopicConjunctions");

            migrationBuilder.DropIndex(
                name: "IX_Comunities_CourseId",
                table: "Comunities");

            migrationBuilder.CreateIndex(
                name: "IX_Comunities_CourseId",
                table: "Comunities",
                column: "CourseId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comunities_CourseId",
                table: "Comunities");

            migrationBuilder.CreateTable(
                name: "TopicConjunctions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NextTopicId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicConjunctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicConjunctions_RecommendTopics_NextTopicId",
                        column: x => x.NextTopicId,
                        principalTable: "RecommendTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopicConjunctions_RecommendTopics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "RecommendTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Comunities_CourseId",
                table: "Comunities",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicConjunctions_NextTopicId",
                table: "TopicConjunctions",
                column: "NextTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicConjunctions_TopicId",
                table: "TopicConjunctions",
                column: "TopicId");
        }
    }
}
