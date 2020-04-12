using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Receiver.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Award = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogicBlock.Translations.Model.EnLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Translation = table.Column<string>(maxLength: 255, nullable: false),
                    WordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogicBlock.Translations.Model.EnLanguage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogicBlock.Translations.Model.EnLanguage_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogicBlock.Translations.Model.RuLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Translation = table.Column<string>(maxLength: 255, nullable: false),
                    WordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogicBlock.Translations.Model.RuLanguage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogicBlock.Translations.Model.RuLanguage_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogicBlock.Translations.Model.EnLanguage_WordId",
                table: "LogicBlock.Translations.Model.EnLanguage",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_LogicBlock.Translations.Model.RuLanguage_WordId",
                table: "LogicBlock.Translations.Model.RuLanguage",
                column: "WordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogicBlock.Translations.Model.EnLanguage");

            migrationBuilder.DropTable(
                name: "LogicBlock.Translations.Model.RuLanguage");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
