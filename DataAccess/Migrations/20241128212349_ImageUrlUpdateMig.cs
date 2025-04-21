using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ImageUrlUpdateMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItemMenuItemImageFile");

            migrationBuilder.DropTable(
                name: "MenuItemImageFile");

            migrationBuilder.AddColumn<string>(
                name: "MenuItemImageUrl",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuItemImageUrl",
                table: "MenuItems");

            migrationBuilder.CreateTable(
                name: "MenuItemImageFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Showcase = table.Column<bool>(type: "bit", nullable: false),
                    Storage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemImageFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemMenuItemImageFile",
                columns: table => new
                {
                    MenuItemsId = table.Column<int>(type: "int", nullable: false),
                    ProductImageFilesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemMenuItemImageFile", x => new { x.MenuItemsId, x.ProductImageFilesId });
                    table.ForeignKey(
                        name: "FK_MenuItemMenuItemImageFile_MenuItemImageFile_ProductImageFilesId",
                        column: x => x.ProductImageFilesId,
                        principalTable: "MenuItemImageFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemMenuItemImageFile_MenuItems_MenuItemsId",
                        column: x => x.MenuItemsId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemMenuItemImageFile_ProductImageFilesId",
                table: "MenuItemMenuItemImageFile",
                column: "ProductImageFilesId");
        }
    }
}
