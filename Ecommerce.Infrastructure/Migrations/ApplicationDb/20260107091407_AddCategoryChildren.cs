using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class AddCategoryChildren : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentId",
                table: "Category",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Category_ParentId",
                table: "Category",
                column: "ParentId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Category_ParentId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_ParentId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Category");


        }
    }
}
