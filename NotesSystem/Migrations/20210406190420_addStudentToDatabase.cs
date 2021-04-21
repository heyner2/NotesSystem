using Microsoft.EntityFrameworkCore.Migrations;

namespace NotesSystem.Migrations
{
    public partial class addStudentToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    idGrade = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gradeText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.idGrade);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    idStudent = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Idgrade = table.Column<int>(type: "int", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    gradeidGrade = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.idStudent);
                    table.ForeignKey(
                        name: "FK_Student_Grade_gradeidGrade",
                        column: x => x.gradeidGrade,
                        principalTable: "Grade",
                        principalColumn: "idGrade",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    idNote = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    note = table.Column<double>(type: "float", nullable: false),
                    idStudent = table.Column<int>(type: "int", nullable: false),
                    studentidStudent = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.idNote);
                    table.ForeignKey(
                        name: "FK_Notes_Student_studentidStudent",
                        column: x => x.studentidStudent,
                        principalTable: "Student",
                        principalColumn: "idStudent",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_studentidStudent",
                table: "Notes",
                column: "studentidStudent");

            migrationBuilder.CreateIndex(
                name: "IX_Student_gradeidGrade",
                table: "Student",
                column: "gradeidGrade");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Grade");
        }
    }
}
