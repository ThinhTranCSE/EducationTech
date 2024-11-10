using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class RemovelearningStyleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearningStyles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LearningStyles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LearnerId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<float>(type: "float", nullable: false),
                    Global = table.Column<float>(type: "float", nullable: false),
                    Intuitive = table.Column<float>(type: "float", nullable: false),
                    Reflective = table.Column<float>(type: "float", nullable: false),
                    Sensing = table.Column<float>(type: "float", nullable: false),
                    Sequential = table.Column<float>(type: "float", nullable: false),
                    Verbal = table.Column<float>(type: "float", nullable: false),
                    Visual = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningStyles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningStyles_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LearningStyles_LearnerId",
                table: "LearningStyles",
                column: "LearnerId");
        }
    }
}
