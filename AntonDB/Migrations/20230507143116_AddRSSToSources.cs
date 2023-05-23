using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AntonDB.Migrations
{
    /// <inheritdoc />
    public partial class AddRSSToSources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceRssURL",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceURl",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceRssURL",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "SourceURl",
                table: "Sources");
        }
    }
}
