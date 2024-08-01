﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RobotInterface.Data;

#nullable disable

namespace RobotInterface.Migrations
{
    [DbContext(typeof(RobotInterfaceContext))]
    [Migration("20240723162626_third")]
    partial class third
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RobotInterface.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("RobotInterface.Models.Command", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommandName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Command");
                });

            modelBuilder.Entity("RobotInterface.Models.Function", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("FunctionName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Function");
                });

            modelBuilder.Entity("RobotInterface.Models.FunctionCommand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CommadnId")
                        .HasColumnType("int");

                    b.Property<int?>("CommandId")
                        .HasColumnType("int");

                    b.Property<int>("FunctionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CommandId");

                    b.HasIndex("FunctionId");

                    b.ToTable("FunctionCommand");
                });

            modelBuilder.Entity("RobotInterface.Models.FunctionLibrary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FunctionId")
                        .HasColumnType("int");

                    b.Property<int>("LibraryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FunctionId");

                    b.HasIndex("LibraryId");

                    b.ToTable("FunctionLibrary");
                });

            modelBuilder.Entity("RobotInterface.Models.Library", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LibraryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Library");
                });

            modelBuilder.Entity("RobotInterface.Models.Function", b =>
                {
                    b.HasOne("RobotInterface.Models.Category", "Category")
                        .WithMany("Functions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("RobotInterface.Models.FunctionCommand", b =>
                {
                    b.HasOne("RobotInterface.Models.Command", "Command")
                        .WithMany("FunctionCommands")
                        .HasForeignKey("CommandId");

                    b.HasOne("RobotInterface.Models.Function", "Function")
                        .WithMany("FunctionCommands")
                        .HasForeignKey("FunctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Command");

                    b.Navigation("Function");
                });

            modelBuilder.Entity("RobotInterface.Models.FunctionLibrary", b =>
                {
                    b.HasOne("RobotInterface.Models.Function", "Function")
                        .WithMany("FunctionLibraries")
                        .HasForeignKey("FunctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RobotInterface.Models.Library", "Library")
                        .WithMany("FunctionLibraries")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Function");

                    b.Navigation("Library");
                });

            modelBuilder.Entity("RobotInterface.Models.Category", b =>
                {
                    b.Navigation("Functions");
                });

            modelBuilder.Entity("RobotInterface.Models.Command", b =>
                {
                    b.Navigation("FunctionCommands");
                });

            modelBuilder.Entity("RobotInterface.Models.Function", b =>
                {
                    b.Navigation("FunctionCommands");

                    b.Navigation("FunctionLibraries");
                });

            modelBuilder.Entity("RobotInterface.Models.Library", b =>
                {
                    b.Navigation("FunctionLibraries");
                });
#pragma warning restore 612, 618
        }
    }
}