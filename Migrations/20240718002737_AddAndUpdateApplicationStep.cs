using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace failure_api.Migrations
{
    /// <inheritdoc />
    public partial class AddAndUpdateApplicationStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobApplicationId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Final = table.Column<bool>(type: "boolean", nullable: false),
                    StepDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Progressed = table.Column<bool>(type: "boolean", nullable: false),
                    ResultDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NextStepId = table.Column<int>(type: "integer", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationSteps_ApplicationSteps_NextStepId",
                        column: x => x.NextStepId,
                        principalTable: "ApplicationSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationSteps_JobApplications_JobApplicationId",
                        column: x => x.JobApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_FirstStepId",
                table: "JobApplications",
                column: "FirstStepId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_UserId",
                table: "JobApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSteps_Id",
                table: "ApplicationSteps",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSteps_JobApplicationId",
                table: "ApplicationSteps",
                column: "JobApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSteps_NextStepId",
                table: "ApplicationSteps",
                column: "NextStepId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_ApplicationSteps_FirstStepId",
                table: "JobApplications",
                column: "FirstStepId",
                principalTable: "ApplicationSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Users_UserId",
                table: "JobApplications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_ApplicationSteps_FirstStepId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Users_UserId",
                table: "JobApplications");

            migrationBuilder.DropTable(
                name: "ApplicationSteps");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_FirstStepId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_UserId",
                table: "JobApplications");
        }
    }
}
