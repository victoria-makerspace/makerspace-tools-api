using Microsoft.EntityFrameworkCore.Migrations;

namespace makerspace_tools_api.Migrations
{
    public partial class AddedToolDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Domain",
                table: "Tools",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Domain",
                table: "Tools");
        }
    }
}
