using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AntonDB.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessLevelIdToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AccessLevel_AccessLevelid",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessLevel",
                table: "AccessLevel");

            migrationBuilder.RenameTable(
                name: "AccessLevel",
                newName: "AccessLevels");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AccessLevels",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessLevels",
                table: "AccessLevels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AccessLevels_AccessLevelid",
                table: "Users",
                column: "AccessLevelid",
                principalTable: "AccessLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AccessLevels_AccessLevelid",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessLevels",
                table: "AccessLevels");

            migrationBuilder.RenameTable(
                name: "AccessLevels",
                newName: "AccessLevel");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AccessLevel",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessLevel",
                table: "AccessLevel",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AccessLevel_AccessLevelid",
                table: "Users",
                column: "AccessLevelid",
                principalTable: "AccessLevel",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
