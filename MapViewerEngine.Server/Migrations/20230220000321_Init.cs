using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapViewerEngine.Server.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Meshes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Data = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meshes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OfficialBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CollectionId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficialBlocks_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficialBlocks_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OfficialItemMeshes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CollectionId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    MeshId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialItemMeshes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficialItemMeshes_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficialItemMeshes_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficialItemMeshes_Meshes_MeshId",
                        column: x => x.MeshId,
                        principalTable: "Meshes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OfficialBlockMeshes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OfficialBlockId = table.Column<int>(type: "int", nullable: false),
                    Ground = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Variant = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    SubVariant = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    MeshId = table.Column<int>(type: "int", nullable: false),
                    CollectionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialBlockMeshes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficialBlockMeshes_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfficialBlockMeshes_Meshes_MeshId",
                        column: x => x.MeshId,
                        principalTable: "Meshes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficialBlockMeshes_OfficialBlocks_OfficialBlockId",
                        column: x => x.OfficialBlockId,
                        principalTable: "OfficialBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialBlockMeshes_CollectionId",
                table: "OfficialBlockMeshes",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialBlockMeshes_MeshId",
                table: "OfficialBlockMeshes",
                column: "MeshId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialBlockMeshes_OfficialBlockId",
                table: "OfficialBlockMeshes",
                column: "OfficialBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialBlocks_AuthorId",
                table: "OfficialBlocks",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialBlocks_CollectionId",
                table: "OfficialBlocks",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialItemMeshes_AuthorId",
                table: "OfficialItemMeshes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialItemMeshes_CollectionId",
                table: "OfficialItemMeshes",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialItemMeshes_MeshId",
                table: "OfficialItemMeshes",
                column: "MeshId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficialBlockMeshes");

            migrationBuilder.DropTable(
                name: "OfficialItemMeshes");

            migrationBuilder.DropTable(
                name: "OfficialBlocks");

            migrationBuilder.DropTable(
                name: "Meshes");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Collections");
        }
    }
}
