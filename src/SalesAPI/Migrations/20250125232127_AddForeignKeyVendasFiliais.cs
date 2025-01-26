using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyVendasFiliais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "Filiais",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Filiais",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "Filiais");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Filiais");
        }
    }
}
