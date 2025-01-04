using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VueAppMvc.Server.Migrations
{
    /// <inheritdoc />
    public partial class BusinessIdToServiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessId",
                table: "services",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "services");
        }
    }
}
