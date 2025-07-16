using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotifyHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOutboxMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationLogs_OutboxMessages_OutboxMessageId",
                table: "NotificationLogs");

            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropIndex(
                name: "IX_NotificationLogs_OutboxMessageId",
                table: "NotificationLogs");

            migrationBuilder.DropColumn(
                name: "OutboxMessageId",
                table: "NotificationLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OutboxMessageId",
                table: "NotificationLogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NotificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Error = table.Column<string>(type: "text", nullable: true),
                    PayloadJson = table.Column<string>(type: "text", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutboxMessages_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLogs_OutboxMessageId",
                table: "NotificationLogs",
                column: "OutboxMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessages_NotificationId",
                table: "OutboxMessages",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationLogs_OutboxMessages_OutboxMessageId",
                table: "NotificationLogs",
                column: "OutboxMessageId",
                principalTable: "OutboxMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
