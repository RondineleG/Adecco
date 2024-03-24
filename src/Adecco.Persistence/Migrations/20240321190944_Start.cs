using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adecco.Persistence.Migrations;

/// <inheritdoc />
public partial class Start : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Clientes",
            columns: table => new
            {
                Id = table
                    .Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Nome = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                Email = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                CPF = table.Column<string>(
                    type: "TEXT",
                    fixedLength: true,
                    maxLength: 11,
                    nullable: false
                ),
                RG = table.Column<string>(
                    type: "TEXT",
                    fixedLength: true,
                    maxLength: 11,
                    nullable: false
                ),
                ContatoId = table.Column<int>(type: "INTEGER", nullable: false),
                EnderecoId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clientes", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Contatos",
            columns: table => new
            {
                Id = table
                    .Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Nome = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                DDD = table.Column<int>(
                    type: "INTEGER",
                    fixedLength: true,
                    maxLength: 2,
                    nullable: false
                ),
                Telefone = table.Column<decimal>(type: "TEXT", maxLength: 9, nullable: false),
                ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                TipoContato = table.Column<byte>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Contatos", x => x.Id);
                table.ForeignKey(
                    name: "FK_Contatos_Clientes_ClienteId",
                    column: x => x.ClienteId,
                    principalTable: "Clientes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            name: "Enderecos",
            columns: table => new
            {
                Id = table
                    .Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Nome = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                CEP = table.Column<string>(
                    type: "TEXT",
                    fixedLength: true,
                    maxLength: 8,
                    nullable: false
                ),
                Logradouro = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                Numero = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                Bairro = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                Complemento = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                Cidade = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                Estado = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                Referencia = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                TipoEndereco = table.Column<byte>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Enderecos", x => x.Id);
                table.ForeignKey(
                    name: "FK_Enderecos_Clientes_ClienteId",
                    column: x => x.ClienteId,
                    principalTable: "Clientes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_Contatos_ClienteId",
            table: "Contatos",
            column: "ClienteId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_Enderecos_ClienteId",
            table: "Enderecos",
            column: "ClienteId"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Contatos");

        migrationBuilder.DropTable(name: "Enderecos");

        migrationBuilder.DropTable(name: "Clientes");
    }
}
