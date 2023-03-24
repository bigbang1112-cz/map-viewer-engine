﻿// <auto-generated />
using System;
using MapViewerEngine.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MapViewerEngine.Server.Migrations
{
    [DbContext(typeof(MapViewerEngineContext))]
    [Migration("20230220000321_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MapViewerEngine.Server.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.Mesh", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<Guid>("Guid")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("Meshes");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.OfficialBlock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("CollectionId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CollectionId");

                    b.ToTable("OfficialBlocks");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.OfficialBlockMesh", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CollectionId")
                        .HasColumnType("int");

                    b.Property<bool>("Ground")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MeshId")
                        .HasColumnType("int");

                    b.Property<int>("OfficialBlockId")
                        .HasColumnType("int");

                    b.Property<byte>("SubVariant")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte>("Variant")
                        .HasColumnType("tinyint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("CollectionId");

                    b.HasIndex("MeshId");

                    b.HasIndex("OfficialBlockId");

                    b.ToTable("OfficialBlockMeshes");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.OfficialItemMesh", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("CollectionId")
                        .HasColumnType("int");

                    b.Property<int>("MeshId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CollectionId");

                    b.HasIndex("MeshId");

                    b.ToTable("OfficialItemMeshes");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.OfficialBlock", b =>
                {
                    b.HasOne("MapViewerEngine.Server.Models.Author", "Author")
                        .WithMany("OfficialBlocks")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MapViewerEngine.Server.Models.Collection", "Collection")
                        .WithMany()
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Collection");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.OfficialBlockMesh", b =>
                {
                    b.HasOne("MapViewerEngine.Server.Models.Collection", null)
                        .WithMany("OfficialBlockMeshes")
                        .HasForeignKey("CollectionId");

                    b.HasOne("MapViewerEngine.Server.Models.Mesh", "Mesh")
                        .WithMany("OfficialBlockMeshes")
                        .HasForeignKey("MeshId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MapViewerEngine.Server.Models.OfficialBlock", "OfficialBlock")
                        .WithMany("OfficialBlockMeshes")
                        .HasForeignKey("OfficialBlockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mesh");

                    b.Navigation("OfficialBlock");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.OfficialItemMesh", b =>
                {
                    b.HasOne("MapViewerEngine.Server.Models.Author", "Author")
                        .WithMany("OfficialItemMeshes")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MapViewerEngine.Server.Models.Collection", "Collection")
                        .WithMany("OfficialItemMeshes")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MapViewerEngine.Server.Models.Mesh", "Mesh")
                        .WithMany("OfficialItemMeshes")
                        .HasForeignKey("MeshId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Collection");

                    b.Navigation("Mesh");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.Author", b =>
                {
                    b.Navigation("OfficialBlocks");

                    b.Navigation("OfficialItemMeshes");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.Collection", b =>
                {
                    b.Navigation("OfficialBlockMeshes");

                    b.Navigation("OfficialItemMeshes");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.Mesh", b =>
                {
                    b.Navigation("OfficialBlockMeshes");

                    b.Navigation("OfficialItemMeshes");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.OfficialBlock", b =>
                {
                    b.Navigation("OfficialBlockMeshes");
                });
#pragma warning restore 612, 618
        }
    }
}
