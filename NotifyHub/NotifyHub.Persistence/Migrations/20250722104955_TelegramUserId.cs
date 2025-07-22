using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotifyHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TelegramUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TelegramTag",
                table: "Users",
                newName: "TelegramUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TelegramUserId",
                table: "Users",
                newName: "TelegramTag");
        }
    }
}
