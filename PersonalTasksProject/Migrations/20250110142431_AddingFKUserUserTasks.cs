using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalTasksProject.Migrations
{
    /// <inheritdoc />
    public partial class AddingFKUserUserTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "user_tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_user_tasks_UserId",
                table: "user_tasks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_user_tasks_users_UserId",
                table: "user_tasks",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_tasks_users_UserId",
                table: "user_tasks");

            migrationBuilder.DropIndex(
                name: "IX_user_tasks_UserId",
                table: "user_tasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "user_tasks");
        }
    }
}
