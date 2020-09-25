using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendsNetwork.Repository.Migrations
{
    public partial class DbInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PhotoUrl = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "state",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PhotoUrl = table.Column<string>(nullable: false),
                    CountryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_state", x => x.Id);
                    table.ForeignKey(
                        name: "FK_state_country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StateId = table.Column<Guid>(nullable: false),
                    CountryId = table.Column<Guid>(nullable: false),
                    Firstname = table.Column<string>(nullable: false),
                    Lastname = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhotoUrl = table.Column<string>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_person_country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_person_state_StateId",
                        column: x => x.StateId,
                        principalTable: "state",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "friendship",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(nullable: false),
                    FriendId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_friendship", x => new { x.PersonId, x.FriendId });
                    table.ForeignKey(
                        name: "FK_friendship_person_FriendId",
                        column: x => x.FriendId,
                        principalTable: "person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_friendship_FriendId",
                table: "friendship",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_person_CountryId",
                table: "person",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_person_StateId",
                table: "person",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_state_CountryId",
                table: "state",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "friendship");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.DropTable(
                name: "state");

            migrationBuilder.DropTable(
                name: "country");
        }
    }
}
