using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PersonalTasksProject.Migrations
{
    /// <inheritdoc />
    public partial class CreateStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "task_priorizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 551, DateTimeKind.Utc).AddTicks(1880)),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 551, DateTimeKind.Utc).AddTicks(2269))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_priorizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UUID", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Password = table.Column<string>(type: "VARCHAR(400)", nullable: false),
                    AvatarUrl = table.Column<string>(type: "VARCHAR(500)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 550, DateTimeKind.Utc).AddTicks(5249)),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 550, DateTimeKind.Utc).AddTicks(5804))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user_tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UUID", nullable: false),
                    TaskPriorizationId = table.Column<int>(type: "INT", nullable: false),
                    UserId = table.Column<Guid>(type: "UUID", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 550, DateTimeKind.Utc).AddTicks(7529)),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP WITH TIME ZONE", nullable: false, defaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 550, DateTimeKind.Utc).AddTicks(8079)),
                    Title = table.Column<string>(type: "VARCHAR(100)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(1000)", maxLength: 200, nullable: false),
                    DueDate = table.Column<DateOnly>(type: "DATE", nullable: false),
                    CompletionDate = table.Column<DateOnly>(type: "DATE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TASKS_USERS_PRIORITIES_TASKS",
                        column: x => x.TaskPriorizationId,
                        principalTable: "task_priorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TASKS_USERS_USERS",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_task_priorizations_Name",
                table: "task_priorizations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_TASK_DUE_DATE",
                table: "user_tasks",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IDX_TASK_PRIORIZATION_ID",
                table: "user_tasks",
                column: "TaskPriorizationId");

            migrationBuilder.CreateIndex(
                name: "IDX_TASK_USER_ID",
                table: "user_tasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "USER_IDX_EMAIL",
                table: "users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_tasks");

            migrationBuilder.DropTable(
                name: "task_priorizations");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
