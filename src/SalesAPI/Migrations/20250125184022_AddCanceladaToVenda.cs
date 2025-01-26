using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SalesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCanceladaToVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cancelada",
                table: "Vendas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FilialId",
                table: "Vendas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Desconto",
                table: "VendaItens",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotalItem",
                table: "VendaItens",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Filiais",
                columns: table => new
                {
                    FilialId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Endereco = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiais", x => x.FilialId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_FilialId",
                table: "Vendas",
                column: "FilialId");

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropTable(
                name: "Filiais");

            migrationBuilder.DropIndex(
                name: "IX_Vendas_FilialId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "Cancelada",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "FilialId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "Desconto",
                table: "VendaItens");

            migrationBuilder.DropColumn(
                name: "ValorTotalItem",
                table: "VendaItens");
        }
    }
}
