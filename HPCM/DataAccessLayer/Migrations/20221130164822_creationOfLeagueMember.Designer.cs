﻿// <auto-generated />
using System;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(HpcmContext))]
    [Migration("20221130164822_creationOfLeagueMember")]
    partial class creationOfLeagueMember
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DataAccessLayer.Models.Announcement", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"), 1L, 1);

                    b.Property<int>("ClubId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .HasPrecision(0)
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("PostId")
                        .HasName("PK__Announce__AA12601834128234");

                    b.HasIndex(new[] { "ClubId" }, "ClubPostRelation");

                    b.HasIndex(new[] { "CreatorId" }, "CreatorRelation");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Club", b =>
                {
                    b.Property<int>("ClubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClubId"), 1L, 1);

                    b.Property<DateTime>("CreationDateTime")
                        .HasPrecision(0)
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PictureUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("Public")
                        .HasColumnType("bit");

                    b.HasKey("ClubId");

                    b.HasIndex(new[] { "OwnerId" }, "OwnerRelation");

                    b.ToTable("Club", (string)null);
                });

            modelBuilder.Entity("DataAccessLayer.Models.ClubMember", b =>
                {
                    b.Property<int>("ClubId")
                        .HasColumnType("int");

                    b.Property<string>("MemberId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Role")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.HasKey("ClubId", "MemberId", "Role")
                        .HasName("ClubMembers_PK");

                    b.HasIndex(new[] { "ClubId" }, "ClubMembersRelation");

                    b.HasIndex(new[] { "MemberId" }, "UserMembersRelation");

                    b.ToTable("ClubMembers");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Invitation", b =>
                {
                    b.Property<int>("ClubId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirationDate")
                        .HasPrecision(0)
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("InvitationHash")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Role")
                        .HasMaxLength(500)
                        .HasColumnType("int");

                    b.HasIndex(new[] { "ClubId" }, "ClubRelation");

                    b.HasIndex(new[] { "MemberId" }, "UserRelation");

                    b.ToTable("Invitation", (string)null);
                });

            modelBuilder.Entity("DataAccessLayer.Models.League", b =>
                {
                    b.Property<int>("LeagueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LeagueId"), 1L, 1);

                    b.Property<int>("ClubId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("Public")
                        .HasColumnType("bit");

                    b.HasKey("LeagueId");

                    b.HasIndex(new[] { "ClubId" }, "ClubLeagueRelation");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("DataAccessLayer.Models.LeagueMember", b =>
                {
                    b.Property<int>("LeagueId")
                        .HasColumnType("int");

                    b.Property<string>("MemberId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LeagueId", "MemberId")
                        .HasName("LeagueMembers_PK");

                    b.HasIndex(new[] { "MemberId" }, "LeagueMembersRelation");

                    b.HasIndex(new[] { "LeagueId" }, "LeagueRelation");

                    b.ToTable("LeagueMember");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Member", b =>
                {
                    b.Property<string>("MemberId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("MemberAssignedType")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ProfilePictureUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("MemberId")
                        .HasName("PK__MemberId");

                    b.HasIndex(new[] { "MemberId" }, "AspNetUserIdRelation");

                    b.ToTable("Member", (string)null);
                });

            modelBuilder.Entity("DataAccessLayer.Models.Tournament", b =>
                {
                    b.Property<int>("TournamentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TournamentId"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("MaxPlayerCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("Public")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartDateTime")
                        .HasPrecision(0)
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("TournamentId");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TournamentEntry", b =>
                {
                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Position")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegistrationDateTime")
                        .HasPrecision(0)
                        .HasColumnType("datetime2(0)");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int")
                        .HasColumnName("TournamentID");

                    b.HasIndex(new[] { "TournamentId" }, "TournamentEntriesRelation");

                    b.HasIndex(new[] { "MemberId" }, "TournamentMembersRelation");

                    b.ToTable("TournamentEntries");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TournamentLink", b =>
                {
                    b.Property<int?>("ClubId")
                        .HasColumnType("int");

                    b.Property<int?>("LeagueId")
                        .HasColumnType("int");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.HasIndex(new[] { "TournamentId" }, "TournamentRelation");

                    b.ToTable("TournamentLink", (string)null);
                });

            modelBuilder.Entity("DataAccessLayer.Models.TournamentReservation", b =>
                {
                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.HasIndex(new[] { "MemberId" }, "MemberReservationRelation");

                    b.HasIndex(new[] { "TournamentId" }, "TournamentReservationRelation");

                    b.ToTable("TournamentReservations");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DataAccessLayer.Models.Announcement", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Club", "Club")
                        .WithMany("Announcements")
                        .HasForeignKey("ClubId")
                        .IsRequired()
                        .HasConstraintName("ClubPostRelation");

                    b.HasOne("DataAccessLayer.Models.Member", "Creator")
                        .WithMany("Announcements")
                        .HasForeignKey("CreatorId")
                        .IsRequired()
                        .HasConstraintName("CreatorRelation");

                    b.Navigation("Club");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Club", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Member", "Owner")
                        .WithMany("Clubs")
                        .HasForeignKey("OwnerId")
                        .IsRequired()
                        .HasConstraintName("OwnerRelation");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("DataAccessLayer.Models.ClubMember", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Club", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId")
                        .IsRequired()
                        .HasConstraintName("ClubMembersRelation");

                    b.HasOne("DataAccessLayer.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .IsRequired()
                        .HasConstraintName("UserMembersRelation");

                    b.Navigation("Club");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Invitation", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Club", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId")
                        .IsRequired()
                        .HasConstraintName("ClubRelation");

                    b.HasOne("DataAccessLayer.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .IsRequired()
                        .HasConstraintName("UserRelation");

                    b.Navigation("Club");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("DataAccessLayer.Models.League", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Club", "Club")
                        .WithMany("Leagues")
                        .HasForeignKey("ClubId")
                        .IsRequired()
                        .HasConstraintName("ClubLeagueRelation");

                    b.Navigation("Club");
                });

            modelBuilder.Entity("DataAccessLayer.Models.LeagueMember", b =>
                {
                    b.HasOne("DataAccessLayer.Models.League", "League")
                        .WithMany()
                        .HasForeignKey("LeagueId")
                        .IsRequired()
                        .HasConstraintName("LeagueRelation");

                    b.HasOne("DataAccessLayer.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .IsRequired()
                        .HasConstraintName("LeagueMembersRelation");

                    b.Navigation("League");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TournamentEntry", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .IsRequired()
                        .HasConstraintName("TournamentMembersRelation");

                    b.HasOne("DataAccessLayer.Models.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .IsRequired()
                        .HasConstraintName("TournamentEntriesRelation");

                    b.Navigation("Member");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TournamentLink", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .IsRequired()
                        .HasConstraintName("TournamentRelation");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("DataAccessLayer.Models.TournamentReservation", b =>
                {
                    b.HasOne("DataAccessLayer.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .IsRequired()
                        .HasConstraintName("UserReservationRelation");

                    b.HasOne("DataAccessLayer.Models.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .IsRequired()
                        .HasConstraintName("TournamentReservationRelation");

                    b.Navigation("Member");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataAccessLayer.Models.Club", b =>
                {
                    b.Navigation("Announcements");

                    b.Navigation("Leagues");
                });

            modelBuilder.Entity("DataAccessLayer.Models.Member", b =>
                {
                    b.Navigation("Announcements");

                    b.Navigation("Clubs");
                });
#pragma warning restore 612, 618
        }
    }
}
