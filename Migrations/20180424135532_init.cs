using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MovinderAPI.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    city = table.Column<string>(maxLength: 60, nullable: false),
                    email = table.Column<string>(nullable: false),
                    name = table.Column<string>(maxLength: 15, nullable: false),
                    passwordHash = table.Column<byte[]>(nullable: true),
                    passwordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invitaiton",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    cinema = table.Column<string>(nullable: false),
                    city = table.Column<string>(nullable: false),
                    inviterId = table.Column<long>(nullable: false),
                    movie = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitaiton", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitaiton_User_inviterId",
                        column: x => x.inviterId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Respond",
                columns: table => new
                {
                    invitationId = table.Column<long>(nullable: false),
                    respondentId = table.Column<long>(nullable: false),
                    status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respond", x => new { x.invitationId, x.respondentId });
                    table.ForeignKey(
                        name: "FK_Respond_Invitaiton_invitationId",
                        column: x => x.invitationId,
                        principalTable: "Invitaiton",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Respond_User_respondentId",
                        column: x => x.respondentId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitaiton_inviterId",
                table: "Invitaiton",
                column: "inviterId");

            migrationBuilder.CreateIndex(
                name: "IX_Respond_respondentId",
                table: "Respond",
                column: "respondentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Respond");

            migrationBuilder.DropTable(
                name: "Invitaiton");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
