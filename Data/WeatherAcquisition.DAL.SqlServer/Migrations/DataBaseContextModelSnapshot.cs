﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeatherAcquisition.DAL.Contexts;

#nullable disable

namespace WeatherAcquisition.DAL.SqlServer.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WeatherAcquisition.DAL.Entities.DataSource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("WeatherAcquisition.DAL.Entities.DataValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("GetedValueDtg")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsFault")
                        .HasColumnType("bit");

                    b.Property<Guid?>("SourceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GetedValueDtg");

                    b.HasIndex("SourceId");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("WeatherAcquisition.DAL.Entities.DataValue", b =>
                {
                    b.HasOne("WeatherAcquisition.DAL.Entities.DataSource", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Source");
                });
#pragma warning restore 612, 618
        }
    }
}
