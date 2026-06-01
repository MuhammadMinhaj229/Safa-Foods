using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SafaFoods.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedLaunchCatalogAndOrderFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "delivery_zones",
                keyColumn: "Id",
                keyValue: new Guid("4f9f0f95-7408-44ad-a6f2-6f30f3e9a101"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 381, DateTimeKind.Unspecified).AddTicks(6321), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 381, DateTimeKind.Unspecified).AddTicks(6328), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "delivery_zones",
                keyColumn: "Id",
                keyValue: new Guid("4f9f0f95-7408-44ad-a6f2-6f30f3e9a102"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 382, DateTimeKind.Unspecified).AddTicks(2302), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 382, DateTimeKind.Unspecified).AddTicks(2304), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "delivery_zones",
                keyColumn: "Id",
                keyValue: new Guid("4f9f0f95-7408-44ad-a6f2-6f30f3e9a103"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 382, DateTimeKind.Unspecified).AddTicks(2318), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 382, DateTimeKind.Unspecified).AddTicks(2318), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "Category", "CreatedAt", "IsActive", "IsSubscriptionEnabled", "Name", "ShopifyProductId", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("7f8f0f95-7408-44ad-a6f2-6f30f3e9a201"), "Fresh Pastes", new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 390, DateTimeKind.Unspecified).AddTicks(2763), new TimeSpan(0, 0, 0, 0, 0)), true, true, "Premium Ginger Garlic Paste", null, "premium-ginger-garlic-paste", new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 390, DateTimeKind.Unspecified).AddTicks(2767), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("7f8f0f95-7408-44ad-a6f2-6f30f3e9a202"), "Fresh Pastes", new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 390, DateTimeKind.Unspecified).AddTicks(6915), new TimeSpan(0, 0, 0, 0, 0)), true, true, "Fresh Garlic Paste", null, "fresh-garlic-paste", new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 390, DateTimeKind.Unspecified).AddTicks(6917), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("7f8f0f95-7408-44ad-a6f2-6f30f3e9a203"), "Fresh Pastes", new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 390, DateTimeKind.Unspecified).AddTicks(6933), new TimeSpan(0, 0, 0, 0, 0)), true, true, "Green Chilli Paste", null, "green-chilli-paste", new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 390, DateTimeKind.Unspecified).AddTicks(6934), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "product_variants",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Label", "Price", "ProductId", "SalePrice", "ShopifyVariantId", "Sku", "UpdatedAt", "Weight" },
                values: new object[,]
                {
                    { new Guid("8f8f0f95-7408-44ad-a6f2-6f30f3e9a301"), new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 393, DateTimeKind.Unspecified).AddTicks(5687), new TimeSpan(0, 0, 0, 0, 0)), true, "200 g", 99m, new Guid("7f8f0f95-7408-44ad-a6f2-6f30f3e9a201"), 94m, null, "SF-GG-200", new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 393, DateTimeKind.Unspecified).AddTicks(5694), new TimeSpan(0, 0, 0, 0, 0)), "200 g" },
                    { new Guid("8f8f0f95-7408-44ad-a6f2-6f30f3e9a302"), new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 393, DateTimeKind.Unspecified).AddTicks(9293), new TimeSpan(0, 0, 0, 0, 0)), true, "500 g", 219m, new Guid("7f8f0f95-7408-44ad-a6f2-6f30f3e9a201"), 209m, null, "SF-GG-500", new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 393, DateTimeKind.Unspecified).AddTicks(9295), new TimeSpan(0, 0, 0, 0, 0)), "500 g" },
                    { new Guid("8f8f0f95-7408-44ad-a6f2-6f30f3e9a303"), new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 393, DateTimeKind.Unspecified).AddTicks(9306), new TimeSpan(0, 0, 0, 0, 0)), true, "200 g", 89m, new Guid("7f8f0f95-7408-44ad-a6f2-6f30f3e9a202"), 84m, null, "SF-GA-200", new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 393, DateTimeKind.Unspecified).AddTicks(9307), new TimeSpan(0, 0, 0, 0, 0)), "200 g" },
                    { new Guid("8f8f0f95-7408-44ad-a6f2-6f30f3e9a304"), new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 393, DateTimeKind.Unspecified).AddTicks(9313), new TimeSpan(0, 0, 0, 0, 0)), true, "500 g", 199m, new Guid("7f8f0f95-7408-44ad-a6f2-6f30f3e9a202"), 189m, null, "SF-GA-500", new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 393, DateTimeKind.Unspecified).AddTicks(9313), new TimeSpan(0, 0, 0, 0, 0)), "500 g" },
                    { new Guid("8f8f0f95-7408-44ad-a6f2-6f30f3e9a305"), new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 393, DateTimeKind.Unspecified).AddTicks(9317), new TimeSpan(0, 0, 0, 0, 0)), true, "150 g", 79m, new Guid("7f8f0f95-7408-44ad-a6f2-6f30f3e9a203"), 75m, null, "SF-GC-150", new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 393, DateTimeKind.Unspecified).AddTicks(9318), new TimeSpan(0, 0, 0, 0, 0)), "150 g" },
                    { new Guid("8f8f0f95-7408-44ad-a6f2-6f30f3e9a306"), new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 393, DateTimeKind.Unspecified).AddTicks(9324), new TimeSpan(0, 0, 0, 0, 0)), true, "300 g", 149m, new Guid("7f8f0f95-7408-44ad-a6f2-6f30f3e9a203"), 142m, null, "SF-GC-300", new DateTimeOffset(new DateTime(2026, 4, 6, 9, 13, 47, 393, DateTimeKind.Unspecified).AddTicks(9324), new TimeSpan(0, 0, 0, 0, 0)), "300 g" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "product_variants",
                keyColumn: "Id",
                keyValue: new Guid("8f8f0f95-7408-44ad-a6f2-6f30f3e9a301"));

            migrationBuilder.DeleteData(
                table: "product_variants",
                keyColumn: "Id",
                keyValue: new Guid("8f8f0f95-7408-44ad-a6f2-6f30f3e9a302"));

            migrationBuilder.DeleteData(
                table: "product_variants",
                keyColumn: "Id",
                keyValue: new Guid("8f8f0f95-7408-44ad-a6f2-6f30f3e9a303"));

            migrationBuilder.DeleteData(
                table: "product_variants",
                keyColumn: "Id",
                keyValue: new Guid("8f8f0f95-7408-44ad-a6f2-6f30f3e9a304"));

            migrationBuilder.DeleteData(
                table: "product_variants",
                keyColumn: "Id",
                keyValue: new Guid("8f8f0f95-7408-44ad-a6f2-6f30f3e9a305"));

            migrationBuilder.DeleteData(
                table: "product_variants",
                keyColumn: "Id",
                keyValue: new Guid("8f8f0f95-7408-44ad-a6f2-6f30f3e9a306"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "Id",
                keyValue: new Guid("7f8f0f95-7408-44ad-a6f2-6f30f3e9a201"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "Id",
                keyValue: new Guid("7f8f0f95-7408-44ad-a6f2-6f30f3e9a202"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "Id",
                keyValue: new Guid("7f8f0f95-7408-44ad-a6f2-6f30f3e9a203"));

            migrationBuilder.UpdateData(
                table: "delivery_zones",
                keyColumn: "Id",
                keyValue: new Guid("4f9f0f95-7408-44ad-a6f2-6f30f3e9a101"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 4, 6, 7, 7, 17, 484, DateTimeKind.Unspecified).AddTicks(2375), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 4, 6, 7, 7, 17, 484, DateTimeKind.Unspecified).AddTicks(2382), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "delivery_zones",
                keyColumn: "Id",
                keyValue: new Guid("4f9f0f95-7408-44ad-a6f2-6f30f3e9a102"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 4, 6, 7, 7, 17, 484, DateTimeKind.Unspecified).AddTicks(7588), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 4, 6, 7, 7, 17, 484, DateTimeKind.Unspecified).AddTicks(7589), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "delivery_zones",
                keyColumn: "Id",
                keyValue: new Guid("4f9f0f95-7408-44ad-a6f2-6f30f3e9a103"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 4, 6, 7, 7, 17, 484, DateTimeKind.Unspecified).AddTicks(7599), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 4, 6, 7, 7, 17, 484, DateTimeKind.Unspecified).AddTicks(7600), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
