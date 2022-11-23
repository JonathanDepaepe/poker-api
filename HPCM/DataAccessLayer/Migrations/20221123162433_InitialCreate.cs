using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoginModel",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MemberType",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MemberTy__516F03B5D856EE9F", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "RegisterModel",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Response",
                columns: table => new
                {
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Public = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MaxPlayerCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.TournamentId);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.MemberId);
                    table.ForeignKey(
                        name: "UserTypeRelation",
                        column: x => x.TypeId,
                        principalTable: "MemberType",
                        principalColumn: "TypeId");
                });

            migrationBuilder.CreateTable(
                name: "TournamentLink",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: true),
                    LeagueId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "TournamentRelation",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId");
                });

            migrationBuilder.CreateTable(
                name: "Club",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Public = table.Column<short>(type: "smallint", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Club", x => x.ClubId);
                    table.ForeignKey(
                        name: "OwnerRelation",
                        column: x => x.OwnerId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                });

            migrationBuilder.CreateTable(
                name: "Jwt",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TokenExpirationDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "UserJWTConnection",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                });

            migrationBuilder.CreateTable(
                name: "TournamentEntries",
                columns: table => new
                {
                    TournamentID = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: true),
                    RegistrationDateTime = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "TournamentEntriesRelation",
                        column: x => x.TournamentID,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId");
                    table.ForeignKey(
                        name: "TournamentMembersRelation",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                });

            migrationBuilder.CreateTable(
                name: "TournamentReservations",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "TournamentReservationRelation",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId");
                    table.ForeignKey(
                        name: "UserReservationRelation",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Announce__AA12601834128234", x => x.PostId);
                    table.ForeignKey(
                        name: "ClubPostRelation",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId");
                    table.ForeignKey(
                        name: "CreatorRelation",
                        column: x => x.CreatorId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                });

            migrationBuilder.CreateTable(
                name: "ClubMembers",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "ClubMembersRelation",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId");
                    table.ForeignKey(
                        name: "UserMembersRelation",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                });

            migrationBuilder.CreateTable(
                name: "Invitation",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    InvitationHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "ClubRelation",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId");
                    table.ForeignKey(
                        name: "UserRelation",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    LeagueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Public = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.LeagueId);
                    table.ForeignKey(
                        name: "ClubLeagueRelation",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId");
                });

            migrationBuilder.CreateIndex(
                name: "ClubPostRelation",
                table: "Announcements",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "CreatorRelation",
                table: "Announcements",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "OwnerRelation",
                table: "Club",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "ClubMembersRelation",
                table: "ClubMembers",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "UserMembersRelation",
                table: "ClubMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "ClubRelation",
                table: "Invitation",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "UserRelation",
                table: "Invitation",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "UserJWTConnection",
                table: "Jwt",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "ClubLeagueRelation",
                table: "Leagues",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "UserTypeRelation",
                table: "Member",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "TournamentEntriesRelation",
                table: "TournamentEntries",
                column: "TournamentID");

            migrationBuilder.CreateIndex(
                name: "TournamentMembersRelation",
                table: "TournamentEntries",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "TournamentRelation",
                table: "TournamentLink",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "MemberReservationRelation",
                table: "TournamentReservations",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "TournamentReservationRelation",
                table: "TournamentReservations",
                column: "TournamentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "ClubMembers");

            migrationBuilder.DropTable(
                name: "Invitation");

            migrationBuilder.DropTable(
                name: "Jwt");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "LoginModel");

            migrationBuilder.DropTable(
                name: "RegisterModel");

            migrationBuilder.DropTable(
                name: "Response");

            migrationBuilder.DropTable(
                name: "TournamentEntries");

            migrationBuilder.DropTable(
                name: "TournamentLink");

            migrationBuilder.DropTable(
                name: "TournamentReservations");

            migrationBuilder.DropTable(
                name: "Club");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "MemberType");
        }
    }
}
