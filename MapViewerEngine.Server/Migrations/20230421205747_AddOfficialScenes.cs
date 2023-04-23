using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapViewerEngine.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddOfficialScenes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfficialScenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CollectionId = table.Column<int>(type: "int", nullable: false),
                    SizeX = table.Column<int>(type: "int", nullable: false),
                    SizeZ = table.Column<int>(type: "int", nullable: false),
                    MeshId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialScenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficialScenes_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficialScenes_Meshes_MeshId",
                        column: x => x.MeshId,
                        principalTable: "Meshes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialScenes_CollectionId",
                table: "OfficialScenes",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialScenes_MeshId",
                table: "OfficialScenes",
                column: "MeshId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficialScenes");
        }
    }
}
