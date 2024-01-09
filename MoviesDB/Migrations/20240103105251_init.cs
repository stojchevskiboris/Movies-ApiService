using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesDB.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) { }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_address_city",
                table: "address");

            migrationBuilder.DropForeignKey(
                name: "fk_staff_address",
                table: "staff");

            migrationBuilder.DropForeignKey(
                name: "fk_store_address",
                table: "store");

            migrationBuilder.DropForeignKey(
                name: "fk_staff_store",
                table: "staff");

            migrationBuilder.DropTable(
                name: "film_actor");

            migrationBuilder.DropTable(
                name: "film_category");

            migrationBuilder.DropTable(
                name: "film_text");

            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropTable(
                name: "actor");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "rental");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "inventory");

            migrationBuilder.DropTable(
                name: "film");

            migrationBuilder.DropTable(
                name: "language");

            migrationBuilder.DropTable(
                name: "city");

            migrationBuilder.DropTable(
                name: "country");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "store");

            migrationBuilder.DropTable(
                name: "staff");
        }
    }
}
