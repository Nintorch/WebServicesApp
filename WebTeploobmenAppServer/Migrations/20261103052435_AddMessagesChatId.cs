using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTeploobmenAppClient.Migrations
{
    /// <inheritdoc />
    public partial class AddMessagesChatId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Messages");
        }
    }
}
