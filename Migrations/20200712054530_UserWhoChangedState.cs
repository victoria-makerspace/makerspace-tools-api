using Microsoft.EntityFrameworkCore.Migrations;

namespace makerspace_tools_api.Migrations
{
    public partial class UserWhoChangedState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WhoChanged",
                table: "ToolStates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhoChanged",
                table: "ToolStates");
        }
    }
}
