using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace makerspace_tools_api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tools",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    HomeLocation = table.Column<string>(nullable: true),
                    Added = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToolStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    WhenChanged = table.Column<DateTime>(nullable: false),
                    ToolId = table.Column<int>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToolStatuses");

            migrationBuilder.DropTable(
                name: "Tools");
        }
    }
}
