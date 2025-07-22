using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotifyHub.NotificationService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TelegramUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "NotificationLogs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "NotificationLogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                table: "NotificationLogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<long>(
                name: "TelegramId",
                table: "NotificationLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "NotificationLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "NotificationLogs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "NotificationLogs");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "NotificationLogs");

            migrationBuilder.DropColumn(
                name: "TelegramId",
                table: "NotificationLogs");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "NotificationLogs");
        }
    }
}
