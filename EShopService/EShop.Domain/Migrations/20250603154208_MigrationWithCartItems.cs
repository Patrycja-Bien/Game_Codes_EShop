using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class MigrationWithCartItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_ShoppingCarts_ShoppingCartUserId",
                table: "CartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem");

            migrationBuilder.RenameTable(
                name: "CartItem",
                newName: "CartItems");

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_ShoppingCartUserId",
                table: "CartItems",
                newName: "IX_CartItems_ShoppingCartUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ShoppingCarts_ShoppingCartUserId",
                table: "CartItems",
                column: "ShoppingCartUserId",
                principalTable: "ShoppingCarts",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ShoppingCarts_ShoppingCartUserId",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "CartItem");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ShoppingCartUserId",
                table: "CartItem",
                newName: "IX_CartItem_ShoppingCartUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_ShoppingCarts_ShoppingCartUserId",
                table: "CartItem",
                column: "ShoppingCartUserId",
                principalTable: "ShoppingCarts",
                principalColumn: "UserId");
        }
    }
}
