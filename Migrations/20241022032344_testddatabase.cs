using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventorySystem.Migrations
{
    /// <inheritdoc />
    public partial class testddatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateHistories_Items_ItemId",
                table: "CreateHistories");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "CreateHistories",
                newName: "DateAdded");

            migrationBuilder.AddColumn<bool>(
                name: "IsBorrowed",
                table: "Items",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsModified",
                table: "Items",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReturned",
                table: "Items",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "CreateHistories",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CreateHistories",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsBorrowed",
                table: "CreateHistories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsModified",
                table: "CreateHistories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "CreateHistories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReturned",
                table: "CreateHistories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateHistories_Items_ItemId",
                table: "CreateHistories",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateHistories_Items_ItemId",
                table: "CreateHistories");

            migrationBuilder.DropColumn(
                name: "IsBorrowed",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsModified",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsReturned",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "CreateHistories");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CreateHistories");

            migrationBuilder.DropColumn(
                name: "IsBorrowed",
                table: "CreateHistories");

            migrationBuilder.DropColumn(
                name: "IsModified",
                table: "CreateHistories");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "CreateHistories");

            migrationBuilder.DropColumn(
                name: "IsReturned",
                table: "CreateHistories");

            migrationBuilder.RenameColumn(
                name: "DateAdded",
                table: "CreateHistories",
                newName: "Timestamp");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateHistories_Items_ItemId",
                table: "CreateHistories",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
