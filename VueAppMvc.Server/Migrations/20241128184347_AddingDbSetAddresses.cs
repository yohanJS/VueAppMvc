using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueAppMvc.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddingDbSetAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bokings_AddressModel_AddressId",
                table: "bokings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bokings",
                table: "bokings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AddressModel",
                table: "AddressModel");

            migrationBuilder.RenameTable(
                name: "bokings",
                newName: "bookings");

            migrationBuilder.RenameTable(
                name: "AddressModel",
                newName: "addresses");

            migrationBuilder.RenameIndex(
                name: "IX_bokings_AddressId",
                table: "bookings",
                newName: "IX_bookings_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookings",
                table: "bookings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_addresses",
                table: "addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_addresses_AddressId",
                table: "bookings",
                column: "AddressId",
                principalTable: "addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_addresses_AddressId",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookings",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_addresses",
                table: "addresses");

            migrationBuilder.RenameTable(
                name: "bookings",
                newName: "bokings");

            migrationBuilder.RenameTable(
                name: "addresses",
                newName: "AddressModel");

            migrationBuilder.RenameIndex(
                name: "IX_bookings_AddressId",
                table: "bokings",
                newName: "IX_bokings_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bokings",
                table: "bokings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AddressModel",
                table: "AddressModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bokings_AddressModel_AddressId",
                table: "bokings",
                column: "AddressId",
                principalTable: "AddressModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
