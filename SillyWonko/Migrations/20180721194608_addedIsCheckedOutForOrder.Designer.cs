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
    [Migration("20180721194608_addedIsCheckedOutForOrder")]
    partial class addedIsCheckedOutForOrder
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SillyWonko.Models.Cart", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsCheckedOut");

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("SillyWonko.Models.CartItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CartID");

                    b.Property<int>("ProductID");

                    b.Property<int>("Quantity");

                    b.HasKey("ID");

                    b.HasIndex("CartID");

                    b.HasIndex("ProductID");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("SillyWonko.Models.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsCheckedOut");

                    b.Property<decimal>("TotalPrice");

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.ToTable("Orders");
                });

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
                        new { ID = 1, Description = "description", Image = "images/candy_one.jpg", Name = "product1", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 2, Description = "description", Image = "images/candy_two.jpg", Name = "product2", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 3, Description = "description", Image = "images/candy_three.jpg", Name = "product3", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 4, Description = "description", Image = "images/candy_four.jpg", Name = "product4", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 5, Description = "description", Image = "images/candy_five.jpg", Name = "product5", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 6, Description = "description", Image = "images/candy_six.jpg", Name = "product6", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 7, Description = "description", Image = "images/candy_seven.jpg", Name = "product7", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 8, Description = "description", Image = "images/candy_eight.jpg", Name = "product8", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 9, Description = "description", Image = "images/candy_nine.jpg", Name = "product9", Price = 9.99m, Sku = "ABCD0001" },
                        new { ID = 10, Description = "description", Image = "images/candy_ten.jpg", Name = "product10", Price = 9.99m, Sku = "ABCD0001" }
                    );
                });

            modelBuilder.Entity("SillyWonko.Models.SoldProduct", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OrderID");

                    b.Property<int>("ProductID");

                    b.Property<int>("Quantity");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.ToTable("SoldProducts");
                });

            modelBuilder.Entity("SillyWonko.Models.CartItem", b =>
                {
                    b.HasOne("SillyWonko.Models.Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SillyWonko.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SillyWonko.Models.SoldProduct", b =>
                {
                    b.HasOne("SillyWonko.Models.Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
