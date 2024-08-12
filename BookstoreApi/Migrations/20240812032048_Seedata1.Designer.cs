﻿// <auto-generated />
using System;
using BookstoreApi.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookstoreApi.Migrations
{
    [DbContext(typeof(BookstoreContext))]
    [Migration("20240812032048_Seedata1")]
    partial class Seedata1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("BookstoreApi.Entities.Author", b =>
                {
                    b.Property<int>("author_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("author_id"));

                    b.Property<string>("author_name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar");

                    b.Property<string>("biography")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar");

                    b.Property<string>("imageUrl")
                        .HasColumnType("longtext");

                    b.HasKey("author_id");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            author_id = 1,
                            author_name = "J.K. Rowling",
                            biography = "British author, best known for the Harry Potter series.",
                            imageUrl = " "
                        },
                        new
                        {
                            author_id = 2,
                            author_name = "J.R.R. Tolkien",
                            biography = "English writer and professor, known for The Hobbit and The Lord of the Rings.",
                            imageUrl = " "
                        });
                });

            modelBuilder.Entity("BookstoreApi.Entities.Book", b =>
                {
                    b.Property<int>("book_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("book_id"));

                    b.Property<int>("author_id")
                        .HasColumnType("int");

                    b.Property<int>("genre_id")
                        .HasColumnType("int");

                    b.Property<string>("imageUrl")
                        .HasColumnType("longtext");

                    b.Property<decimal>("price")
                        .HasColumnType("decimal(5,2)");

                    b.Property<DateTime>("publication_date")
                        .HasColumnType("date");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar");

                    b.HasKey("book_id");

                    b.HasIndex("author_id");

                    b.HasIndex("genre_id");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            book_id = 1,
                            author_id = 1,
                            genre_id = 1,
                            imageUrl = " ",
                            price = 19.99m,
                            publication_date = new DateTime(1997, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            title = "Harry Potter and the Sorcerer's Stone"
                        },
                        new
                        {
                            book_id = 2,
                            author_id = 2,
                            genre_id = 1,
                            imageUrl = " ",
                            price = 14.99m,
                            publication_date = new DateTime(1937, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            title = "The Hobbit"
                        });
                });

            modelBuilder.Entity("BookstoreApi.Entities.Genre", b =>
                {
                    b.Property<int>("genre_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("genre_id"));

                    b.Property<string>("genre_name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar");

                    b.HasKey("genre_id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            genre_id = 1,
                            genre_name = "Fantasy"
                        },
                        new
                        {
                            genre_id = 2,
                            genre_name = "Science Fiction"
                        });
                });

            modelBuilder.Entity("BookstoreApi.Entities.Book", b =>
                {
                    b.HasOne("BookstoreApi.Entities.Author", "Author")
                        .WithMany()
                        .HasForeignKey("author_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookstoreApi.Entities.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("genre_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Genre");
                });
#pragma warning restore 612, 618
        }
    }
}
