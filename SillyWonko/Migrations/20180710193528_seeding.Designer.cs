﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SillyWonko.Data;

namespace SillyWonko.Migrations
{
    [DbContext(typeof(WonkoDbContext))]
    [Migration("20180710193528_seeding")]
    partial class seeding
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SillyWonko.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("Price");

                    b.Property<string>("Sku")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Products");

                    b.HasData(
                        new { ID = 1, Description = "description", Image = "", Name = "product1", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 2, Description = "description", Image = "", Name = "product2", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 3, Description = "description", Image = "", Name = "product3", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 4, Description = "description", Image = "", Name = "product4", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 5, Description = "description", Image = "", Name = "product5", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 6, Description = "description", Image = "", Name = "product6", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 7, Description = "description", Image = "", Name = "product7", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 8, Description = "description", Image = "", Name = "product8", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 9, Description = "description", Image = "", Name = "product9", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 10, Description = "description", Image = "", Name = "product10", Price = 9.99m, Sku = "ABCD0001" }
                    );
                });
#pragma warning restore 612, 618
        }
    }
}
