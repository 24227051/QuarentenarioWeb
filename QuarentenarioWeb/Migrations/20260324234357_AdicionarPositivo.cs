using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuarentenarioWeb.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarPositivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // adicionar a coluna "positivo" do tipo booleano à tabela "Analise", com valor padrão "false"
            migrationBuilder.AddColumn<bool>(
                name: "Positivo",
                table: "Analise",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // adicionar a coluna "positivo" do tipo booleano à tabela "AnaliseDetalhe", com valor padrão "false"
            migrationBuilder.AddColumn<bool>(
                name: "Positivo",
                table: "AnaliseDetalhe",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // excluir a coluna "positivo" do tipo booleano à tabela "Analise"
            migrationBuilder.DropColumn(
                name: "Positivo",
                table: "Analise");

            // excluir a coluna "positivo" do tipo booleano à tabela "AnaliseDetalhe"
            migrationBuilder.DropColumn(
                name: "Positivo",
                table: "AnaliseDetalhe");
        }
    }
}
