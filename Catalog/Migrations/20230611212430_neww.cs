using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Migrations
{
    public partial class neww : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credits_Categories_CategoryId",
                table: "Credits");

            migrationBuilder.DropIndex(
                name: "IX_Credits_CategoryId",
                table: "Credits");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Credits_CategoryId",
                table: "Credits",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credits_Categories_CategoryId",
                table: "Credits",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
