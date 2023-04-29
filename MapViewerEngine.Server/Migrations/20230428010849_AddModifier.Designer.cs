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
    [Migration("20230428010849_AddModifier")]
    partial class AddModifier
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Nadeo"
                        });
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Speed"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Alpine"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Rally"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Island"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Bay"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Coast"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Stadium"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Canyon"
                        },
                        new
                        {
                            Id = 9,
                            Name = "StadiumMP4"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Valley"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Lagoon"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Stadium2020"
                        });
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.Mesh", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("mediumblob");

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

                    b.Property<byte[]>("Meta")
                        .IsRequired()
                        .HasColumnType("blob");

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

            modelBuilder.Entity("MapViewerEngine.Server.Models.OfficialScene", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CollectionId")
                        .HasColumnType("int");

                    b.Property<int>("MeshId")
                        .HasColumnType("int");

                    b.Property<int>("SizeX")
                        .HasColumnType("int");

                    b.Property<int>("SizeZ")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CollectionId");

                    b.HasIndex("MeshId");

                    b.ToTable("OfficialScenes");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.OfficialShader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("blob");

                    b.Property<string>("Modifier")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("OfficialShaders");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.OfficialBlock", b =>
                {
                    b.HasOne("MapViewerEngine.Server.Models.Author", "Author")
                        .WithMany()
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
                    b.HasOne("MapViewerEngine.Server.Models.Mesh", "Mesh")
                        .WithMany()
                        .HasForeignKey("MeshId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MapViewerEngine.Server.Models.OfficialBlock", "OfficialBlock")
                        .WithMany()
                        .HasForeignKey("OfficialBlockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mesh");

                    b.Navigation("OfficialBlock");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.OfficialItemMesh", b =>
                {
                    b.HasOne("MapViewerEngine.Server.Models.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MapViewerEngine.Server.Models.Collection", "Collection")
                        .WithMany()
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MapViewerEngine.Server.Models.Mesh", "Mesh")
                        .WithMany()
                        .HasForeignKey("MeshId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Collection");

                    b.Navigation("Mesh");
                });

            modelBuilder.Entity("MapViewerEngine.Server.Models.OfficialScene", b =>
                {
                    b.HasOne("MapViewerEngine.Server.Models.Collection", "Collection")
                        .WithMany()
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MapViewerEngine.Server.Models.Mesh", "Mesh")
                        .WithMany()
                        .HasForeignKey("MeshId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collection");

                    b.Navigation("Mesh");
                });
#pragma warning restore 612, 618
        }
    }
}
