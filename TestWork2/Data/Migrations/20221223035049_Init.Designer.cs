﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestWork2.Data;

#nullable disable

namespace TestWork2.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221223035049_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TestWork2.Data.Models.File", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("TestWork2.Data.Models.FileResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("AverageIndicator")
                        .HasColumnType("float");

                    b.Property<double>("AverageSeconds")
                        .HasColumnType("float");

                    b.Property<TimeSpan>("ElapsedTime")
                        .HasColumnType("time");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("MaxIndicator")
                        .HasColumnType("float");

                    b.Property<double>("MedianSeconds")
                        .HasColumnType("float");

                    b.Property<DateTime>("MinDateTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("MinIndicator")
                        .HasColumnType("float");

                    b.Property<int>("RowCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FileName");

                    b.ToTable("FileResults");
                });

            modelBuilder.Entity("TestWork2.Data.Models.FileValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Indicator")
                        .HasColumnType("float");

                    b.Property<int>("Seconds")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FileName");

                    b.ToTable("FileValues");
                });

            modelBuilder.Entity("TestWork2.Data.Models.FileResult", b =>
                {
                    b.HasOne("TestWork2.Data.Models.File", "File")
                        .WithMany()
                        .HasForeignKey("FileName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");
                });

            modelBuilder.Entity("TestWork2.Data.Models.FileValue", b =>
                {
                    b.HasOne("TestWork2.Data.Models.File", "File")
                        .WithMany()
                        .HasForeignKey("FileName");

                    b.Navigation("File");
                });
#pragma warning restore 612, 618
        }
    }
}
