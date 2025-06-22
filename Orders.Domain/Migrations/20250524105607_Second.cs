using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orders.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Address_ShippingAddressAddressId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressAddressId",
                table: "Orders",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShippingAddressAddressId",
                table: "Orders",
                newName: "IX_Orders_AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Address_AddressId",
                table: "Orders",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Address_AddressId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "Orders",
                newName: "ShippingAddressAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                newName: "IX_Orders_ShippingAddressAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Address_ShippingAddressAddressId",
                table: "Orders",
                column: "ShippingAddressAddressId",
                principalTable: "Address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
