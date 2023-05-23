using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AntonDB.Migrations
{
    /// <inheritdoc />
    public partial class AddAcessLevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessLevelid",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccessLevel",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLevel", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccessLevelid",
                table: "Users",
                column: "AccessLevelid");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AccessLevel_AccessLevelid",
                table: "Users",
                column: "AccessLevelid",
                principalTable: "AccessLevel",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AccessLevel_AccessLevelid",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AccessLevel");

            migrationBuilder.DropIndex(
                name: "IX_Users_AccessLevelid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AccessLevelid",
                table: "Users");
        }
    }
}
