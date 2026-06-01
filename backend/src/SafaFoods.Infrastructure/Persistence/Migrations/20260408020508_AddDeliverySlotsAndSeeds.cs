using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SafaFoods.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliverySlotsAndSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,");

            migrationBuilder.AddColumn<Guid>(
                name: "DeliverySlotId",
                table: "orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ScheduledDeliveryDate",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliverySlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MaxOrders = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliverySlots", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DeliverySlots",
                columns: new[] { "Id", "CreatedAt", "EndTime", "IsActive", "Label", "MaxOrders", "StartTime" },
                values: new object[,]
                {
                    { new Guid("9f8f0f95-7408-44ad-a6f2-6f30f3e9a401"), new DateTimeOffset(new DateTime(2026, 4, 8, 2, 5, 6, 170, DateTimeKind.Unspecified).AddTicks(332), new TimeSpan(0, 0, 0, 0, 0)), new TimeSpan(0, 10, 0, 0, 0), true, "Morning (7 AM - 10 AM)", 50, new TimeSpan(0, 7, 0, 0, 0) },
                    { new Guid("9f8f0f95-7408-44ad-a6f2-6f30f3e9a402"), new DateTimeOffset(new DateTime(2026, 4, 8, 2, 5, 6, 170, DateTimeKind.Unspecified).AddTicks(2510), new TimeSpan(0, 0, 0, 0, 0)), new TimeSpan(0, 16, 0, 0, 0), true, "Afternoon (1 PM - 4 PM)", 50, new TimeSpan(0, 13, 0, 0, 0) },
                    { new Guid("9f8f0f95-7408-44ad-a6f2-6f30f3e9a403"), new DateTimeOffset(new DateTime(2026, 4, 8, 2, 5, 6, 170, DateTimeKind.Unspecified).AddTicks(2516), new TimeSpan(0, 0, 0, 0, 0)), new TimeSpan(0, 21, 0, 0, 0), true, "Evening (6 PM - 9 PM)", 30, new TimeSpan(0, 18, 0, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_DeliverySlotId",
                table: "orders",
                column: "DeliverySlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_DeliverySlots_DeliverySlotId",
                table: "orders",
                column: "DeliverySlotId",
                principalTable: "DeliverySlots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_DeliverySlots_DeliverySlotId",
                table: "orders");

            migrationBuilder.DropTable(
                name: "DeliverySlots");

            migrationBuilder.DropIndex(
                name: "IX_orders_DeliverySlotId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "DeliverySlotId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "ScheduledDeliveryDate",
                table: "orders");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:pg_trgm", ",,");
        }
    }
}
