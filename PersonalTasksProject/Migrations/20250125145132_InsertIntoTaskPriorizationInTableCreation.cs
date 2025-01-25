using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersonalTasksProject.Migrations
{
    /// <inheritdoc />
    public partial class InsertIntoTaskPriorizationInTableCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "users",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(2641),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 550, DateTimeKind.Utc).AddTicks(5804));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "users",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(2274),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 550, DateTimeKind.Utc).AddTicks(5249));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "user_tasks",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(4822),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 550, DateTimeKind.Utc).AddTicks(8079));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "user_tasks",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(4357),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 550, DateTimeKind.Utc).AddTicks(7529));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "task_priorizations",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8513),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 551, DateTimeKind.Utc).AddTicks(2269));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "task_priorizations",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8144),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 551, DateTimeKind.Utc).AddTicks(1880));

            migrationBuilder.InsertData(
                table: "task_priorizations",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8956), "Critical", new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8963) },
                    { 2, new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8969), "High", new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8973) },
                    { 3, new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8977), "Medium", new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8981) },
                    { 4, new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8985), "Low", new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8989) },
                    { 5, new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8993), "None", new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8996) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "task_priorizations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "task_priorizations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "task_priorizations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "task_priorizations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "task_priorizations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "users",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 550, DateTimeKind.Utc).AddTicks(5804),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValue: new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(2641));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "users",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 550, DateTimeKind.Utc).AddTicks(5249),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValue: new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(2274));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "user_tasks",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 550, DateTimeKind.Utc).AddTicks(8079),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValue: new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(4822));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "user_tasks",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 550, DateTimeKind.Utc).AddTicks(7529),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValue: new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(4357));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "task_priorizations",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 551, DateTimeKind.Utc).AddTicks(2269),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValue: new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8513));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "task_priorizations",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 24, 19, 39, 8, 551, DateTimeKind.Utc).AddTicks(1880),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE",
                oldDefaultValue: new DateTime(2025, 1, 25, 14, 51, 32, 200, DateTimeKind.Utc).AddTicks(8144));
        }
    }
}
