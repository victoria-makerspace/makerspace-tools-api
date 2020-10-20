using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace makerspace_tools_api.Migrations
{
    public partial class StatusToState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToolStatuses");

            migrationBuilder.CreateTable(
                name: "ToolStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    State = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    WhenChanged = table.Column<DateTime>(nullable: false),
                    ToolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToolStates_Tools_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToolStates_ToolId",
                table: "ToolStates",
                column: "ToolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToolStates");

            migrationBuilder.CreateTable(
                name: "ToolStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    ToolId = table.Column<int>(type: "INTEGER", nullable: false),
                    WhenChanged = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToolStatuses_Tools_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToolStatuses_ToolId",
                table: "ToolStatuses",
                column: "ToolId");
        }
    }
}
