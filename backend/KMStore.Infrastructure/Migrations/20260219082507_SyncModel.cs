using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatAt",
                table: "Products",
                newName: "CreatedAT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAT",
                table: "Products",
                newName: "CreatAt");
        }
    }
}
