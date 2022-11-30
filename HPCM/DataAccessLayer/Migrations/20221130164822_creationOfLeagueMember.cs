using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class creationOfLeagueMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "LeagueMember",
                columns: table => new
                {
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("LeagueMembers_PK", x => new { x.LeagueId, x.MemberId });
                    table.ForeignKey(
                        name: "LeagueMembersRelation",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                    table.ForeignKey(
                        name: "LeagueRelation",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "LeagueId");
                });

            migrationBuilder.CreateIndex(
                name: "LeagueMembersRelation",
                table: "LeagueMember",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "LeagueRelation",
                table: "LeagueMember",
                column: "LeagueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueMember");

            migrationBuilder.DropPrimaryKey(
                name: "ClubMembers_PK",
                table: "ClubMembers");
        }
    }
}
