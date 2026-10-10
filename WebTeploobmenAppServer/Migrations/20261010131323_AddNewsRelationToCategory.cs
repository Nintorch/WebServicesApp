using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTeploobmenAppClient.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsRelationToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "News");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "News",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "News");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "News",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
