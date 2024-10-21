using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EducationTech.Migrations
{
    public partial class CreateNewRecommendTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CourseCode",
                table: "Courses",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CourseGroupId",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Credits",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecommendedSemester",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CourseGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    MinCredits = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroups", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PrerequisiteCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    PrerequisiteCourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrerequisiteCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrerequisiteCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrerequisiteCourses_Courses_PrerequisiteCourseId",
                        column: x => x.PrerequisiteCourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialities_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CourseSpecialities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SpecialityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSpecialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseSpecialities_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSpecialities_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_BranchId",
                table: "Courses",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseCode",
                table: "Courses",
                column: "CourseCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseGroupId",
                table: "Courses",
                column: "CourseGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSpecialities_CourseId",
                table: "CourseSpecialities",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSpecialities_SpecialityId",
                table: "CourseSpecialities",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_PrerequisiteCourses_CourseId",
                table: "PrerequisiteCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_PrerequisiteCourses_PrerequisiteCourseId",
                table: "PrerequisiteCourses",
                column: "PrerequisiteCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_BranchId",
                table: "Specialities",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Branches_BranchId",
                table: "Courses",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseGroups_CourseGroupId",
                table: "Courses",
                column: "CourseGroupId",
                principalTable: "CourseGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Branches_BranchId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseGroups_CourseGroupId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "CourseGroups");

            migrationBuilder.DropTable(
                name: "CourseSpecialities");

            migrationBuilder.DropTable(
                name: "PrerequisiteCourses");

            migrationBuilder.DropTable(
                name: "Specialities");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Courses_BranchId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseCode",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseGroupId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseGroupId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Credits",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "RecommendedSemester",
                table: "Courses");
        }
    }
}
