using Microsoft.EntityFrameworkCore.Migrations;

namespace Receiver.Migrations
{
    public partial class RenameTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogicBlock.Translations.Model.EnLanguage_Words_WordId",
                table: "LogicBlock.Translations.Model.EnLanguage");

            migrationBuilder.DropForeignKey(
                name: "FK_LogicBlock.Translations.Model.RuLanguage_Words_WordId",
                table: "LogicBlock.Translations.Model.RuLanguage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogicBlock.Translations.Model.RuLanguage",
                table: "LogicBlock.Translations.Model.RuLanguage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogicBlock.Translations.Model.EnLanguage",
                table: "LogicBlock.Translations.Model.EnLanguage");

            migrationBuilder.RenameTable(
                name: "LogicBlock.Translations.Model.RuLanguage",
                newName: "RuLanguage");

            migrationBuilder.RenameTable(
                name: "LogicBlock.Translations.Model.EnLanguage",
                newName: "EnLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_LogicBlock.Translations.Model.RuLanguage_WordId",
                table: "RuLanguage",
                newName: "IX_RuLanguage_WordId");

            migrationBuilder.RenameIndex(
                name: "IX_LogicBlock.Translations.Model.EnLanguage_WordId",
                table: "EnLanguage",
                newName: "IX_EnLanguage_WordId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RuLanguage",
                table: "RuLanguage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnLanguage",
                table: "EnLanguage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnLanguage_Words_WordId",
                table: "EnLanguage",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RuLanguage_Words_WordId",
                table: "RuLanguage",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnLanguage_Words_WordId",
                table: "EnLanguage");

            migrationBuilder.DropForeignKey(
                name: "FK_RuLanguage_Words_WordId",
                table: "RuLanguage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RuLanguage",
                table: "RuLanguage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnLanguage",
                table: "EnLanguage");

            migrationBuilder.RenameTable(
                name: "RuLanguage",
                newName: "LogicBlock.Translations.Model.RuLanguage");

            migrationBuilder.RenameTable(
                name: "EnLanguage",
                newName: "LogicBlock.Translations.Model.EnLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_RuLanguage_WordId",
                table: "LogicBlock.Translations.Model.RuLanguage",
                newName: "IX_LogicBlock.Translations.Model.RuLanguage_WordId");

            migrationBuilder.RenameIndex(
                name: "IX_EnLanguage_WordId",
                table: "LogicBlock.Translations.Model.EnLanguage",
                newName: "IX_LogicBlock.Translations.Model.EnLanguage_WordId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogicBlock.Translations.Model.RuLanguage",
                table: "LogicBlock.Translations.Model.RuLanguage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogicBlock.Translations.Model.EnLanguage",
                table: "LogicBlock.Translations.Model.EnLanguage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LogicBlock.Translations.Model.EnLanguage_Words_WordId",
                table: "LogicBlock.Translations.Model.EnLanguage",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LogicBlock.Translations.Model.RuLanguage_Words_WordId",
                table: "LogicBlock.Translations.Model.RuLanguage",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
