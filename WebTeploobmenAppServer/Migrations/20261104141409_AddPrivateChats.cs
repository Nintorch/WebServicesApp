using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTeploobmenAppClient.Migrations
{
    /// <inheritdoc />
    public partial class AddPrivateChats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrivateChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Person1Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Person2Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateChats_Users_Person1Id",
                        column: x => x.Person1Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrivateChats_Users_Person2Id",
                        column: x => x.Person2Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChats_Person1Id",
                table: "PrivateChats",
                column: "Person1Id");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChats_Person2Id",
                table: "PrivateChats",
                column: "Person2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivateChats");
        }
    }
}
