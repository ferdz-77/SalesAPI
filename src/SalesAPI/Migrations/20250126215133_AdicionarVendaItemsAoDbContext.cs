using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarVendaItemsAoDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendaItens_Produtos_ProdutoId",
                table: "VendaItens");

            migrationBuilder.DropColumn(
                name: "PrecoUnitario",
                table: "VendaItens");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "VendaItens");

            migrationBuilder.RenameColumn(
                name: "ValorTotalItem",
                table: "VendaItens",
                newName: "PrecoOriginal");

            migrationBuilder.AddForeignKey(
                name: "FK_VendaItens_Produtos_ProdutoId",
                table: "VendaItens",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendaItens_Produtos_ProdutoId",
                table: "VendaItens");

            migrationBuilder.RenameColumn(
                name: "PrecoOriginal",
                table: "VendaItens",
                newName: "ValorTotalItem");

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoUnitario",
                table: "VendaItens",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "VendaItens",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_VendaItens_Produtos_ProdutoId",
                table: "VendaItens",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
