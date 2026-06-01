using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafaFoods.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerWallets_customers_CustomerId",
                table: "CustomerWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_CustomerWallets_WalletId",
                table: "WalletTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerWallets",
                table: "CustomerWallets");

            migrationBuilder.RenameTable(
                name: "CustomerWallets",
                newName: "customer_wallets");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerWallets_CustomerId",
                table: "customer_wallets",
                newName: "IX_customer_wallets_CustomerId");

            migrationBuilder.AddColumn<Guid>(
                name: "WalletId1",
                table: "WalletTransactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "product_variants",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "customer_wallets",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId1",
                table: "customer_wallets",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "customer_wallets",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddPrimaryKey(
                name: "PK_customer_wallets",
                table: "customer_wallets",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "outbox_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    AggregateType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    PayloadJson = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    LastError = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_events", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "Id",
                keyValue: new Guid("9f8f0f95-7408-44ad-a6f2-6f30f3e9a401"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2026, 4, 10, 16, 31, 49, 643, DateTimeKind.Unspecified).AddTicks(9420), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "Id",
                keyValue: new Guid("9f8f0f95-7408-44ad-a6f2-6f30f3e9a402"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2026, 4, 10, 16, 31, 49, 644, DateTimeKind.Unspecified).AddTicks(1644), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "Id",
                keyValue: new Guid("9f8f0f95-7408-44ad-a6f2-6f30f3e9a403"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2026, 4, 10, 16, 31, 49, 644, DateTimeKind.Unspecified).AddTicks(1650), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletId1",
                table: "WalletTransactions",
                column: "WalletId1");

            migrationBuilder.CreateIndex(
                name: "IX_orders_Status",
                table: "orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_customer_wallets_CustomerId1",
                table: "customer_wallets",
                column: "CustomerId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_outbox_events_AggregateType_AggregateId_OccurredAt",
                table: "outbox_events",
                columns: new[] { "AggregateType", "AggregateId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_outbox_events_Status_OccurredAt",
                table: "outbox_events",
                columns: new[] { "Status", "OccurredAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_customer_wallets_customers_CustomerId",
                table: "customer_wallets",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_customer_wallets_customers_CustomerId1",
                table: "customer_wallets",
                column: "CustomerId1",
                principalTable: "customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_customer_wallets_WalletId",
                table: "WalletTransactions",
                column: "WalletId",
                principalTable: "customer_wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_customer_wallets_WalletId1",
                table: "WalletTransactions",
                column: "WalletId1",
                principalTable: "customer_wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customer_wallets_customers_CustomerId",
                table: "customer_wallets");

            migrationBuilder.DropForeignKey(
                name: "FK_customer_wallets_customers_CustomerId1",
                table: "customer_wallets");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_customer_wallets_WalletId",
                table: "WalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_customer_wallets_WalletId1",
                table: "WalletTransactions");

            migrationBuilder.DropTable(
                name: "outbox_events");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_WalletId1",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_orders_Status",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customer_wallets",
                table: "customer_wallets");

            migrationBuilder.DropIndex(
                name: "IX_customer_wallets_CustomerId1",
                table: "customer_wallets");

            migrationBuilder.DropColumn(
                name: "WalletId1",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "customer_wallets");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "customer_wallets");

            migrationBuilder.RenameTable(
                name: "customer_wallets",
                newName: "CustomerWallets");

            migrationBuilder.RenameIndex(
                name: "IX_customer_wallets_CustomerId",
                table: "CustomerWallets",
                newName: "IX_CustomerWallets_CustomerId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "CustomerWallets",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldDefaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerWallets",
                table: "CustomerWallets",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "Id",
                keyValue: new Guid("9f8f0f95-7408-44ad-a6f2-6f30f3e9a401"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2026, 4, 8, 2, 5, 6, 170, DateTimeKind.Unspecified).AddTicks(332), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "Id",
                keyValue: new Guid("9f8f0f95-7408-44ad-a6f2-6f30f3e9a402"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2026, 4, 8, 2, 5, 6, 170, DateTimeKind.Unspecified).AddTicks(2510), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "Id",
                keyValue: new Guid("9f8f0f95-7408-44ad-a6f2-6f30f3e9a403"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2026, 4, 8, 2, 5, 6, 170, DateTimeKind.Unspecified).AddTicks(2516), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerWallets_customers_CustomerId",
                table: "CustomerWallets",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_CustomerWallets_WalletId",
                table: "WalletTransactions",
                column: "WalletId",
                principalTable: "CustomerWallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
