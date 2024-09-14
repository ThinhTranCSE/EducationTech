using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class CreateRecommendationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Learners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    BackgroundKnowledge = table.Column<int>(type: "int", nullable: false),
                    Qualification = table.Column<int>(type: "int", nullable: false),
                    Branch = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Learners", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LearningObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false),
                    Structure = table.Column<int>(type: "int", nullable: false),
                    AggregationLevel = table.Column<int>(type: "int", nullable: false),
                    Format = table.Column<int>(type: "int", nullable: false),
                    LearningResourceType = table.Column<int>(type: "int", nullable: false),
                    InteractivityType = table.Column<int>(type: "int", nullable: false),
                    InteractivityLevel = table.Column<int>(type: "int", nullable: false),
                    SemanticDensity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningObjects", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LearningStyles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<int>(type: "int", nullable: false),
                    Reflective = table.Column<int>(type: "int", nullable: false),
                    Sensing = table.Column<int>(type: "int", nullable: false),
                    Intuitive = table.Column<int>(type: "int", nullable: false),
                    Visual = table.Column<int>(type: "int", nullable: false),
                    Verbal = table.Column<int>(type: "int", nullable: false),
                    Sequential = table.Column<int>(type: "int", nullable: false),
                    Global = table.Column<int>(type: "int", nullable: false),
                    LearnerId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "LearnerLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LearnerId = table.Column<int>(type: "int", nullable: false),
                    LearningObjectId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnerLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearnerLogs_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearnerLogs_LearningObjects_LearningObjectId",
                        column: x => x.LearningObjectId,
                        principalTable: "LearningObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerLogs_LearnerId",
                table: "LearnerLogs",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerLogs_LearningObjectId",
                table: "LearnerLogs",
                column: "LearningObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningStyles_LearnerId",
                table: "LearningStyles",
                column: "LearnerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearnerLogs");

            migrationBuilder.DropTable(
                name: "LearningStyles");

            migrationBuilder.DropTable(
                name: "LearningObjects");

            migrationBuilder.DropTable(
                name: "Learners");
        }
    }
}
