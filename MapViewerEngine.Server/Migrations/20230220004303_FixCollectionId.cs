using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapViewerEngine.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixCollectionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficialBlockMeshes_Collections_CollectionId",
                table: "OfficialBlockMeshes");

            migrationBuilder.DropIndex(
                name: "IX_OfficialBlockMeshes_CollectionId",
                table: "OfficialBlockMeshes");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "OfficialBlockMeshes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CollectionId",
                table: "OfficialBlockMeshes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficialBlockMeshes_CollectionId",
                table: "OfficialBlockMeshes",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficialBlockMeshes_Collections_CollectionId",
                table: "OfficialBlockMeshes",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id");
        }
    }
}
