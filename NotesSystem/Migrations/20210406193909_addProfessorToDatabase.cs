using Microsoft.EntityFrameworkCore.Migrations;

namespace NotesSystem.Migrations
{
    public partial class addProfessorToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfessoridProfessor",
                table: "Grade",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "professor",
                columns: table => new
                {
                    idProfessor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professor", x => x.idProfessor);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grade_ProfessoridProfessor",
                table: "Grade",
                column: "ProfessoridProfessor");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_professor_ProfessoridProfessor",
                table: "Grade",
                column: "ProfessoridProfessor",
                principalTable: "professor",
                principalColumn: "idProfessor",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_professor_ProfessoridProfessor",
                table: "Grade");

            migrationBuilder.DropTable(
                name: "professor");

            migrationBuilder.DropIndex(
                name: "IX_Grade_ProfessoridProfessor",
                table: "Grade");

            migrationBuilder.DropColumn(
                name: "ProfessoridProfessor",
                table: "Grade");
        }
    }
}
