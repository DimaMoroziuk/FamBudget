using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcelGen.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddedAccessFunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Purchase",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Income",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Access",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccessRecieverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccessType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Access", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Access_AspNetUsers_AccessRecieverId",
                        column: x => x.AccessRecieverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Access_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_AuthorId",
                table: "Purchase",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Income_AuthorId",
                table: "Income",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Access_AccessRecieverId",
                table: "Access",
                column: "AccessRecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_Access_AuthorId",
                table: "Access",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Income_AspNetUsers_AuthorId",
                table: "Income",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchase_AspNetUsers_AuthorId",
                table: "Purchase",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Income_AspNetUsers_AuthorId",
                table: "Income");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchase_AspNetUsers_AuthorId",
                table: "Purchase");

            migrationBuilder.DropTable(
                name: "Access");

            migrationBuilder.DropIndex(
                name: "IX_Purchase_AuthorId",
                table: "Purchase");

            migrationBuilder.DropIndex(
                name: "IX_Income_AuthorId",
                table: "Income");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Purchase");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Income");
        }
    }
}
