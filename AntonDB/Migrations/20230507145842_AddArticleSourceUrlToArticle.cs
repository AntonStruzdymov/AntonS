using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AntonDB.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleSourceUrlToArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArticleSourceURL",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleSourceURL",
                table: "Articles");
        }
    }
}
