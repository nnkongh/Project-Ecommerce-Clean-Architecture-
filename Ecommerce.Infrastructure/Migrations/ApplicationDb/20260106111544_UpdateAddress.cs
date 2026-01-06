using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class UpdateAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "Address_PostalCode",
                table: "User",
                newName: "Address_Street");

            migrationBuilder.RenameColumn(
                name: "Address_PostalCode",
                table: "Orders",
                newName: "Address_Street");

            migrationBuilder.AddColumn<string>(
                name: "Address_Province",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Province",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Province",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Address_Province",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "User",
                newName: "Address_PostalCode");

            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "Orders",
                newName: "Address_PostalCode");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
