using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventorySystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateHistories_Items_ItemId",
                table: "CreateHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateHistories_Items_ItemId",
                table: "CreateHistories",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateHistories_Items_ItemId",
                table: "CreateHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateHistories_Items_ItemId",
                table: "CreateHistories",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
