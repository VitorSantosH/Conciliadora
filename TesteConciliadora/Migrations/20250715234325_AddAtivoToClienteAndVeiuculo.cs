using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesteConciliadora.Migrations
{
    /// <inheritdoc />
    public partial class AddAtivoToClienteAndVeiuculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Veiculos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Veiculos");
        }
    }
}
