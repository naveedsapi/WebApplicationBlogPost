using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplicationBlogPost.Migrations
{
    /// <inheritdoc />
    public partial class SeedingInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Technology" },
                    { 2, null, "Health" },
                    { 3, null, "LifStyle" },
                    { 4, null, "Education" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Author", "CategoryId", "Content", "FeathureImagePath", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, "Sapi", 1, "Technology Blog Post Content 1", "Web_image.png", new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Technology Post Content 1" },
                    { 2, "Sapi", 2, "Health Blog Post Content 1", "Web_image.png", new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Health Post Content 1" },
                    { 3, "Sapi", 3, "LifeStyle Blog Post Content 1", "Web_image.png", new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "LifeStyle Post Content 1" },
                    { 4, "Sapi", 4, "Education Blog Post Content 1", "Web_image.png", new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Education Post Content 1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
