﻿// <auto-generated />
using Cities.API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cities.API.Migrations
{
    [DbContext(typeof(CityInfoContext))]
    [Migration("20210727174113_SampleData")]
    partial class SampleData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Cities.API.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new { Id = 1, Description = "The one with that big park.", Name = "New York City" },
                        new { Id = 2, Description = "The one with the cathedral that was never really finished.", Name = "Antwerp" },
                        new { Id = 3, Description = "The one with that big tower.", Name = "Paris" }
                    );
                });

            modelBuilder.Entity("Cities.API.Entities.PointOfInterest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId");

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("PointsOfInterest");

                    b.HasData(
                        new { Id = 1, CityId = 1, Description = "The most visited urban park in the United States.", Nome = "Central Park" },
                        new { Id = 2, CityId = 1, Description = "A 102-story skyscraper located in Midtown Manhattan.", Nome = "Empire State Building" },
                        new { Id = 3, CityId = 2, Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans.", Nome = "Cathedral" },
                        new { Id = 4, CityId = 2, Description = "The the finest example of railway architecture in Belgium.", Nome = "Antwerp Central Station" },
                        new { Id = 5, CityId = 3, Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel.", Nome = "Eiffel Tower" },
                        new { Id = 6, CityId = 3, Description = "The world's largest museum.", Nome = "The Louvre" }
                    );
                });

            modelBuilder.Entity("Cities.API.Entities.PointOfInterest", b =>
                {
                    b.HasOne("Cities.API.Entities.City", "City")
                        .WithMany("PointsOfInterest")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}