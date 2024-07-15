using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace failure_api.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdFollowed = table.Column<string>(type: "text", nullable: false),
                    IdFollowing = table.Column<string>(type: "text", nullable: false),
                    FollowDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Allowed = table.Column<bool>(type: "boolean", nullable: false),
                    AllowDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follows_Users_IdFollowed",
                        column: x => x.IdFollowed,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Follows_Users_IdFollowing",
                        column: x => x.IdFollowing,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Follows_IdFollowed_IdFollowing",
                table: "Follows",
                columns: new[] { "IdFollowed", "IdFollowing" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Follows_IdFollowing",
                table: "Follows",
                column: "IdFollowing");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Follows");
        }
    }
}
