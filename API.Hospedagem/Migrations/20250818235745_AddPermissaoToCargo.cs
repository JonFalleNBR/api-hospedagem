using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Hospedagem.Migrations
{
    public partial class AddPermissaoToCargo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adiciona a coluna Permissao na tabela Cargo, se não existir
            migrationBuilder.AddColumn<int>(
                name: "Permissao",
                table: "Cargo",
                type: "int",
                nullable: false,
                defaultValue: 2);

            // Adiciona o constraint de verificação na coluna Permissao
            migrationBuilder.AddCheckConstraint(
                name: "CK_Cargo_Permissao",
                table: "Cargo",
                sql: "[Permissao] IN (1,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove o constraint de verificação
            migrationBuilder.DropCheckConstraint(
                name: "CK_Cargo_Permissao",
                table: "Cargo");

            // Remove a coluna Permissao
            migrationBuilder.DropColumn(
                name: "Permissao",
                table: "Cargo");
        }
    }
}