using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class AddedKeysToInvites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "LeagueRelation",
                table: "LeagueMembers",
                newName: "LeagueRelation1");

            migrationBuilder.RenameIndex(
                name: "UserRelation",
                table: "LeagueInvitation",
                newName: "UserRelation1");

            migrationBuilder.AddPrimaryKey(
                name: "LeagueInvitation_PK",
                table: "LeagueInvitation",
                columns: new[] { "LeagueId", "MemberId" });

            migrationBuilder.AddPrimaryKey(
                name: "ClubInvitation_PK",
                table: "Invitation",
                columns: new[] { "ClubId", "MemberId", "Role" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "LeagueInvitation_PK",
                table: "LeagueInvitation");

            migrationBuilder.DropPrimaryKey(
                name: "ClubInvitation_PK",
                table: "Invitation");

            migrationBuilder.RenameIndex(
                name: "LeagueRelation1",
                table: "LeagueMembers",
                newName: "LeagueRelation");

            migrationBuilder.RenameIndex(
                name: "UserRelation1",
                table: "LeagueInvitation",
                newName: "UserRelation");
        }
    }
}
