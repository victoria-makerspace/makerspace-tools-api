﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using makerspace_tools_api.Data;

namespace makerspace_tools_api.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200207123530_AddedToolDomain")]
    partial class AddedToolDomain
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("makerspace_tools_api.Models.Tool", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Added")
                        .HasColumnType("TEXT");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Domain")
                        .HasColumnType("TEXT");

                    b.Property<string>("HomeLocation")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Tools");
                });

            modelBuilder.Entity("makerspace_tools_api.Models.ToolState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<string>("State")
                        .HasColumnType("TEXT");

                    b.Property<int>("ToolId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("WhenChanged")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ToolId");

                    b.ToTable("ToolStates");
                });

            modelBuilder.Entity("makerspace_tools_api.Models.ToolState", b =>
                {
                    b.HasOne("makerspace_tools_api.Models.Tool", "Tool")
                        .WithMany("StateHistory")
                        .HasForeignKey("ToolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
