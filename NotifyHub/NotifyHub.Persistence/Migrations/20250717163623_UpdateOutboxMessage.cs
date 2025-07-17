using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotifyHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOutboxMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessages_NotificationId",
                table: "OutboxMessages",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OutboxMessages_Notifications_NotificationId",
                table: "OutboxMessages",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutboxMessages_Notifications_NotificationId",
                table: "OutboxMessages");

            migrationBuilder.DropIndex(
                name: "IX_OutboxMessages_NotificationId",
                table: "OutboxMessages");
        }
    }
}
