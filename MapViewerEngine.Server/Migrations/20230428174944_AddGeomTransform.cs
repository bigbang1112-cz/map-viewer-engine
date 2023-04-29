using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapViewerEngine.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddGeomTransform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "GeomRotationX",
                table: "OfficialBlockMeshes",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "GeomRotationY",
                table: "OfficialBlockMeshes",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "GeomRotationZ",
                table: "OfficialBlockMeshes",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "GeomTranslationX",
                table: "OfficialBlockMeshes",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "GeomTranslationY",
                table: "OfficialBlockMeshes",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "GeomTranslationZ",
                table: "OfficialBlockMeshes",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeomRotationX",
                table: "OfficialBlockMeshes");

            migrationBuilder.DropColumn(
                name: "GeomRotationY",
                table: "OfficialBlockMeshes");

            migrationBuilder.DropColumn(
                name: "GeomRotationZ",
                table: "OfficialBlockMeshes");

            migrationBuilder.DropColumn(
                name: "GeomTranslationX",
                table: "OfficialBlockMeshes");

            migrationBuilder.DropColumn(
                name: "GeomTranslationY",
                table: "OfficialBlockMeshes");

            migrationBuilder.DropColumn(
                name: "GeomTranslationZ",
                table: "OfficialBlockMeshes");
        }
    }
}
