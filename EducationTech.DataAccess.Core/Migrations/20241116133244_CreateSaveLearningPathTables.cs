using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class CreateSaveLearningPathTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseLearningPathOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    LearnerId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLearningPathOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseLearningPathOrders_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseLearningPathOrders_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LearningObjectLearningPathOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LearningObjectId = table.Column<int>(type: "int", nullable: false),
                    LearnerId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningObjectLearningPathOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningObjectLearningPathOrders_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningObjectLearningPathOrders_LearningObjects_LearningObj~",
                        column: x => x.LearningObjectId,
                        principalTable: "LearningObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TopicLearningPathOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    LearnerId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicLearningPathOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicLearningPathOrders_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopicLearningPathOrders_RecommendTopics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "RecommendTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLearningPathOrders_CourseId_LearnerId",
                table: "CourseLearningPathOrders",
                columns: new[] { "CourseId", "LearnerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseLearningPathOrders_LearnerId",
                table: "CourseLearningPathOrders",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningObjectLearningPathOrders_LearnerId",
                table: "LearningObjectLearningPathOrders",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningObjectLearningPathOrders_LearningObjectId_LearnerId",
                table: "LearningObjectLearningPathOrders",
                columns: new[] { "LearningObjectId", "LearnerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopicLearningPathOrders_LearnerId",
                table: "TopicLearningPathOrders",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicLearningPathOrders_TopicId_LearnerId",
                table: "TopicLearningPathOrders",
                columns: new[] { "TopicId", "LearnerId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseLearningPathOrders");

            migrationBuilder.DropTable(
                name: "LearningObjectLearningPathOrders");

            migrationBuilder.DropTable(
                name: "TopicLearningPathOrders");
        }
    }
}
