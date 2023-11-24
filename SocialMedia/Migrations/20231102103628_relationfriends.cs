using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class relationfriends : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Friends_FirstId",
                table: "Friends");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FirstId_SecondId",
                table: "Friends",
                columns: new[] { "FirstId", "SecondId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Friends_FirstId_SecondId",
                table: "Friends");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FirstId",
                table: "Friends",
                column: "FirstId");
        }
    }
}
