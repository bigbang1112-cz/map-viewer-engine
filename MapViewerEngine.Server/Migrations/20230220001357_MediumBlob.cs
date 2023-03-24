using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapViewerEngine.Server.Migrations
{
    /// <inheritdoc />
    public partial class MediumBlob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Meta",
                table: "OfficialBlocks",
                type: "blob",
                nullable: false);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Data",
                table: "Meshes",
                type: "mediumblob",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "longblob");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meta",
                table: "OfficialBlocks");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Data",
                table: "Meshes",
                type: "longblob",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "mediumblob");
        }
    }
}
