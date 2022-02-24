﻿// <auto-generated />
using MVC_FILE_VIEW_.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MVC_FILE_VIEW_.Migrations
{
    [DbContext(typeof(dbcontextclass))]
    [Migration("20211225164024_NewSeed")]
    partial class NewSeed
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MVC_FILE_VIEW_.Model.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Hoppies")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Hoppies = 0,
                            Name = "Mohamed"
                        },
                        new
                        {
                            Id = 2,
                            Hoppies = 1,
                            Name = "Essam"
                        },
                        new
                        {
                            Id = 3,
                            Hoppies = 2,
                            Name = "Walid"
                        },
                        new
                        {
                            Id = 4,
                            Hoppies = 2,
                            Name = "Hamada"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
