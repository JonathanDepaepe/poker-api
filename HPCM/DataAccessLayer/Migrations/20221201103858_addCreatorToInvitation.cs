using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class addCreatorToInvitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "LeagueMember",
                newName: "LeagueMembers");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Invitation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Invitation");

            migrationBuilder.RenameTable(
                name: "LeagueMembers",
                newName: "LeagueMember");
        }
    }
}
