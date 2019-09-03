using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeOlho.Search.Politico.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PalavraSubstituta",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Palavra = table.Column<string>(nullable: true),
                    Substituta = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PalavraSubstituta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoliticoMandato",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CPF = table.Column<long>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    Apelido = table.Column<string>(nullable: true),
                    Ano = table.Column<int>(nullable: false),
                    Partido = table.Column<string>(nullable: true),
                    Cargo = table.Column<string>(nullable: true),
                    Abrangencia = table.Column<string>(nullable: true),
                    Eleito = table.Column<bool>(nullable: false),
                    TermoPesquisa = table.Column<string>("Text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticoMandato", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PalavraSubstituta_Palavra",
                table: "PalavraSubstituta",
                column: "Palavra",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PoliticoMandato_CPF_Ano",
                table: "PoliticoMandato",
                columns: new[] { "CPF", "Ano" },
                unique: true);

            migrationBuilder.Sql("Alter Table PoliticoMandato Add FullText(TermoPesquisa)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PalavraSubstituta");

            migrationBuilder.DropTable(
                name: "PoliticoMandato");
        }
    }
}
