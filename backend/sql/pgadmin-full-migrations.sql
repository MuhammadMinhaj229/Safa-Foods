CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE TABLE admin_users (
        "Id" uuid NOT NULL,
        "Name" character varying(120) NOT NULL,
        "Email" character varying(160) NOT NULL,
        "Role" integer NOT NULL,
        "IsActive" boolean NOT NULL,
        "LastLoginAt" timestamp with time zone,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_admin_users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE TABLE customers (
        "Id" uuid NOT NULL,
        "ShopifyCustomerId" character varying(64),
        "FullName" character varying(160) NOT NULL,
        "Phone" character varying(20) NOT NULL,
        "Email" character varying(160) NOT NULL,
        "MarketingOptIn" boolean NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_customers" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE TABLE delivery_zones (
        "Id" uuid NOT NULL,
        "Name" character varying(80) NOT NULL,
        "MinKm" numeric(5,2) NOT NULL,
        "MaxKm" numeric(5,2),
        "Fee" numeric(8,2) NOT NULL,
        "EstimatedMinMinutes" integer NOT NULL,
        "EstimatedMaxMinutes" integer NOT NULL,
        "IsActive" boolean NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_delivery_zones" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE TABLE products (
        "Id" uuid NOT NULL,
        "ShopifyProductId" character varying(64),
        "Slug" character varying(180) NOT NULL,
        "Name" character varying(160) NOT NULL,
        "Category" character varying(120) NOT NULL,
        "IsSubscriptionEnabled" boolean NOT NULL,
        "IsActive" boolean NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_products" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE TABLE notification_logs (
        "Id" uuid NOT NULL,
        "CustomerId" uuid,
        "Channel" integer NOT NULL,
        "EventType" character varying(80) NOT NULL,
        "Recipient" character varying(160) NOT NULL,
        "MessageStatus" character varying(50) NOT NULL,
        "ProviderReference" character varying(120),
        "PayloadJson" text,
        "CreatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_notification_logs" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_notification_logs_customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES customers ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE TABLE addresses (
        "Id" uuid NOT NULL,
        "CustomerId" uuid NOT NULL,
        "FullName" character varying(160) NOT NULL,
        "Phone" character varying(20) NOT NULL,
        "AddressLine1" character varying(240) NOT NULL,
        "AddressLine2" character varying(240),
        "Landmark" character varying(160),
        "Area" character varying(120) NOT NULL,
        "City" character varying(120) NOT NULL,
        "Pincode" character varying(12) NOT NULL,
        "Latitude" numeric(9,6),
        "Longitude" numeric(9,6),
        "DistanceKm" numeric(5,2),
        "IsDefault" boolean NOT NULL,
        "IsServiceable" boolean NOT NULL,
        "DeliveryZoneId" uuid,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_addresses" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_addresses_customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES customers ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_addresses_delivery_zones_DeliveryZoneId" FOREIGN KEY ("DeliveryZoneId") REFERENCES delivery_zones ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE TABLE product_variants (
        "Id" uuid NOT NULL,
        "ProductId" uuid NOT NULL,
        "ShopifyVariantId" character varying(64),
        "Label" character varying(80) NOT NULL,
        "Weight" character varying(40) NOT NULL,
        "Price" numeric(10,2) NOT NULL,
        "SalePrice" numeric(10,2),
        "Sku" character varying(80) NOT NULL,
        "IsActive" boolean NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_product_variants" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_product_variants_products_ProductId" FOREIGN KEY ("ProductId") REFERENCES products ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE TABLE orders (
        "Id" uuid NOT NULL,
        "ShopifyOrderId" character varying(64),
        "CustomerId" uuid NOT NULL,
        "OrderType" integer NOT NULL,
        "Subtotal" numeric(10,2) NOT NULL,
        "DiscountTotal" numeric(10,2) NOT NULL,
        "DeliveryFee" numeric(10,2) NOT NULL,
        "GrandTotal" numeric(10,2) NOT NULL,
        "PaymentMethod" integer NOT NULL,
        "PaymentStatus" integer NOT NULL,
        "Status" integer NOT NULL,
        "AddressId" uuid NOT NULL,
        "DeliveryZoneId" uuid,
        "PlacedAt" timestamp with time zone NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_orders" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_orders_addresses_AddressId" FOREIGN KEY ("AddressId") REFERENCES addresses ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_orders_customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES customers ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_orders_delivery_zones_DeliveryZoneId" FOREIGN KEY ("DeliveryZoneId") REFERENCES delivery_zones ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE TABLE subscriptions (
        "Id" uuid NOT NULL,
        "CustomerId" uuid NOT NULL,
        "ProductId" uuid NOT NULL,
        "VariantId" uuid NOT NULL,
        "PlanCode" character varying(50) NOT NULL,
        "BillingCycle" character varying(40) NOT NULL,
        "DeliveriesInCycle" integer NOT NULL,
        "QuantityPerDelivery" integer NOT NULL,
        "PlanPrice" numeric(10,2) NOT NULL,
        "DiscountPercent" numeric(5,2) NOT NULL,
        "StartDate" timestamp with time zone NOT NULL,
        "EndDate" timestamp with time zone,
        "NextDeliveryDate" timestamp with time zone NOT NULL,
        "NextBillingDate" timestamp with time zone,
        "Status" integer NOT NULL,
        "AddressId" uuid NOT NULL,
        "DeliveryZoneId" uuid,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_subscriptions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_subscriptions_addresses_AddressId" FOREIGN KEY ("AddressId") REFERENCES addresses ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_subscriptions_customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES customers ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_subscriptions_delivery_zones_DeliveryZoneId" FOREIGN KEY ("DeliveryZoneId") REFERENCES delivery_zones ("Id"),
        CONSTRAINT "FK_subscriptions_product_variants_VariantId" FOREIGN KEY ("VariantId") REFERENCES product_variants ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_subscriptions_products_ProductId" FOREIGN KEY ("ProductId") REFERENCES products ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE TABLE order_items (
        "Id" uuid NOT NULL,
        "OrderId" uuid NOT NULL,
        "ProductId" uuid NOT NULL,
        "VariantId" uuid NOT NULL,
        "Quantity" integer NOT NULL,
        "UnitPrice" numeric(10,2) NOT NULL,
        "TotalPrice" numeric(10,2) NOT NULL,
        CONSTRAINT "PK_order_items" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_order_items_orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES orders ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_order_items_product_variants_VariantId" FOREIGN KEY ("VariantId") REFERENCES product_variants ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_order_items_products_ProductId" FOREIGN KEY ("ProductId") REFERENCES products ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE TABLE subscription_deliveries (
        "Id" uuid NOT NULL,
        "SubscriptionId" uuid NOT NULL,
        "ScheduledDate" timestamp with time zone NOT NULL,
        "Quantity" integer NOT NULL,
        "DeliveryFee" numeric(10,2) NOT NULL,
        "Status" integer NOT NULL,
        "FulfilledOrderId" uuid,
        "AttemptCount" integer NOT NULL,
        "DeliveredAt" timestamp with time zone,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_subscription_deliveries" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_subscription_deliveries_orders_FulfilledOrderId" FOREIGN KEY ("FulfilledOrderId") REFERENCES orders ("Id"),
        CONSTRAINT "FK_subscription_deliveries_subscriptions_SubscriptionId" FOREIGN KEY ("SubscriptionId") REFERENCES subscriptions ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    INSERT INTO delivery_zones ("Id", "CreatedAt", "EstimatedMaxMinutes", "EstimatedMinMinutes", "Fee", "IsActive", "MaxKm", "MinKm", "Name", "UpdatedAt")
    VALUES ('4f9f0f95-7408-44ad-a6f2-6f30f3e9a101', TIMESTAMPTZ '2026-04-06T07:07:17.484237+00:00', 45, 30, 10.0, TRUE, 3.0, 0.0, 'Zone A', TIMESTAMPTZ '2026-04-06T07:07:17.484238+00:00');
    INSERT INTO delivery_zones ("Id", "CreatedAt", "EstimatedMaxMinutes", "EstimatedMinMinutes", "Fee", "IsActive", "MaxKm", "MinKm", "Name", "UpdatedAt")
    VALUES ('4f9f0f95-7408-44ad-a6f2-6f30f3e9a102', TIMESTAMPTZ '2026-04-06T07:07:17.484758+00:00', 70, 45, 20.0, TRUE, 8.0, 3.0, 'Zone B', TIMESTAMPTZ '2026-04-06T07:07:17.484758+00:00');
    INSERT INTO delivery_zones ("Id", "CreatedAt", "EstimatedMaxMinutes", "EstimatedMinMinutes", "Fee", "IsActive", "MaxKm", "MinKm", "Name", "UpdatedAt")
    VALUES ('4f9f0f95-7408-44ad-a6f2-6f30f3e9a103', TIMESTAMPTZ '2026-04-06T07:07:17.484759+00:00', 110, 70, 40.0, TRUE, 12.0, 8.0, 'Zone C', TIMESTAMPTZ '2026-04-06T07:07:17.48476+00:00');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_addresses_CustomerId" ON addresses ("CustomerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_addresses_DeliveryZoneId" ON addresses ("DeliveryZoneId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_admin_users_Email" ON admin_users ("Email");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_customers_Email" ON customers ("Email");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_customers_Phone" ON customers ("Phone");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_customers_ShopifyCustomerId" ON customers ("ShopifyCustomerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_notification_logs_CustomerId" ON notification_logs ("CustomerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_order_items_OrderId" ON order_items ("OrderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_order_items_ProductId" ON order_items ("ProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_order_items_VariantId" ON order_items ("VariantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_orders_AddressId" ON orders ("AddressId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_orders_CustomerId" ON orders ("CustomerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_orders_DeliveryZoneId" ON orders ("DeliveryZoneId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_orders_ShopifyOrderId" ON orders ("ShopifyOrderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_product_variants_ProductId" ON product_variants ("ProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_product_variants_ShopifyVariantId" ON product_variants ("ShopifyVariantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_product_variants_Sku" ON product_variants ("Sku");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_products_ShopifyProductId" ON products ("ShopifyProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_products_Slug" ON products ("Slug");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_subscription_deliveries_FulfilledOrderId" ON subscription_deliveries ("FulfilledOrderId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_subscription_deliveries_SubscriptionId" ON subscription_deliveries ("SubscriptionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_subscriptions_AddressId" ON subscriptions ("AddressId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_subscriptions_CustomerId" ON subscriptions ("CustomerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_subscriptions_DeliveryZoneId" ON subscriptions ("DeliveryZoneId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_subscriptions_ProductId" ON subscriptions ("ProductId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    CREATE INDEX "IX_subscriptions_VariantId" ON subscriptions ("VariantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406070718_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260406070718_InitialCreate', '9.0.1');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406091349_SeedLaunchCatalogAndOrderFlow') THEN
    UPDATE delivery_zones SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:13:47.381632+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:13:47.381632+00:00'
    WHERE "Id" = '4f9f0f95-7408-44ad-a6f2-6f30f3e9a101';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406091349_SeedLaunchCatalogAndOrderFlow') THEN
    UPDATE delivery_zones SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:13:47.38223+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:13:47.38223+00:00'
    WHERE "Id" = '4f9f0f95-7408-44ad-a6f2-6f30f3e9a102';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406091349_SeedLaunchCatalogAndOrderFlow') THEN
    UPDATE delivery_zones SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:13:47.382231+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:13:47.382231+00:00'
    WHERE "Id" = '4f9f0f95-7408-44ad-a6f2-6f30f3e9a103';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406091349_SeedLaunchCatalogAndOrderFlow') THEN
    INSERT INTO products ("Id", "Category", "CreatedAt", "IsActive", "IsSubscriptionEnabled", "Name", "ShopifyProductId", "Slug", "UpdatedAt")
    VALUES ('7f8f0f95-7408-44ad-a6f2-6f30f3e9a201', 'Fresh Pastes', TIMESTAMPTZ '2026-04-06T09:13:47.390276+00:00', TRUE, TRUE, 'Premium Ginger Garlic Paste', NULL, 'premium-ginger-garlic-paste', TIMESTAMPTZ '2026-04-06T09:13:47.390276+00:00');
    INSERT INTO products ("Id", "Category", "CreatedAt", "IsActive", "IsSubscriptionEnabled", "Name", "ShopifyProductId", "Slug", "UpdatedAt")
    VALUES ('7f8f0f95-7408-44ad-a6f2-6f30f3e9a202', 'Fresh Pastes', TIMESTAMPTZ '2026-04-06T09:13:47.390691+00:00', TRUE, TRUE, 'Fresh Garlic Paste', NULL, 'fresh-garlic-paste', TIMESTAMPTZ '2026-04-06T09:13:47.390691+00:00');
    INSERT INTO products ("Id", "Category", "CreatedAt", "IsActive", "IsSubscriptionEnabled", "Name", "ShopifyProductId", "Slug", "UpdatedAt")
    VALUES ('7f8f0f95-7408-44ad-a6f2-6f30f3e9a203', 'Fresh Pastes', TIMESTAMPTZ '2026-04-06T09:13:47.390693+00:00', TRUE, TRUE, 'Green Chilli Paste', NULL, 'green-chilli-paste', TIMESTAMPTZ '2026-04-06T09:13:47.390693+00:00');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406091349_SeedLaunchCatalogAndOrderFlow') THEN
    INSERT INTO product_variants ("Id", "CreatedAt", "IsActive", "Label", "Price", "ProductId", "SalePrice", "ShopifyVariantId", "Sku", "UpdatedAt", "Weight")
    VALUES ('8f8f0f95-7408-44ad-a6f2-6f30f3e9a301', TIMESTAMPTZ '2026-04-06T09:13:47.393568+00:00', TRUE, '200 g', 99.0, '7f8f0f95-7408-44ad-a6f2-6f30f3e9a201', 94.0, NULL, 'SF-GG-200', TIMESTAMPTZ '2026-04-06T09:13:47.393569+00:00', '200 g');
    INSERT INTO product_variants ("Id", "CreatedAt", "IsActive", "Label", "Price", "ProductId", "SalePrice", "ShopifyVariantId", "Sku", "UpdatedAt", "Weight")
    VALUES ('8f8f0f95-7408-44ad-a6f2-6f30f3e9a302', TIMESTAMPTZ '2026-04-06T09:13:47.393929+00:00', TRUE, '500 g', 219.0, '7f8f0f95-7408-44ad-a6f2-6f30f3e9a201', 209.0, NULL, 'SF-GG-500', TIMESTAMPTZ '2026-04-06T09:13:47.393929+00:00', '500 g');
    INSERT INTO product_variants ("Id", "CreatedAt", "IsActive", "Label", "Price", "ProductId", "SalePrice", "ShopifyVariantId", "Sku", "UpdatedAt", "Weight")
    VALUES ('8f8f0f95-7408-44ad-a6f2-6f30f3e9a303', TIMESTAMPTZ '2026-04-06T09:13:47.39393+00:00', TRUE, '200 g', 89.0, '7f8f0f95-7408-44ad-a6f2-6f30f3e9a202', 84.0, NULL, 'SF-GA-200', TIMESTAMPTZ '2026-04-06T09:13:47.39393+00:00', '200 g');
    INSERT INTO product_variants ("Id", "CreatedAt", "IsActive", "Label", "Price", "ProductId", "SalePrice", "ShopifyVariantId", "Sku", "UpdatedAt", "Weight")
    VALUES ('8f8f0f95-7408-44ad-a6f2-6f30f3e9a304', TIMESTAMPTZ '2026-04-06T09:13:47.393931+00:00', TRUE, '500 g', 199.0, '7f8f0f95-7408-44ad-a6f2-6f30f3e9a202', 189.0, NULL, 'SF-GA-500', TIMESTAMPTZ '2026-04-06T09:13:47.393931+00:00', '500 g');
    INSERT INTO product_variants ("Id", "CreatedAt", "IsActive", "Label", "Price", "ProductId", "SalePrice", "ShopifyVariantId", "Sku", "UpdatedAt", "Weight")
    VALUES ('8f8f0f95-7408-44ad-a6f2-6f30f3e9a305', TIMESTAMPTZ '2026-04-06T09:13:47.393931+00:00', TRUE, '150 g', 79.0, '7f8f0f95-7408-44ad-a6f2-6f30f3e9a203', 75.0, NULL, 'SF-GC-150', TIMESTAMPTZ '2026-04-06T09:13:47.393931+00:00', '150 g');
    INSERT INTO product_variants ("Id", "CreatedAt", "IsActive", "Label", "Price", "ProductId", "SalePrice", "ShopifyVariantId", "Sku", "UpdatedAt", "Weight")
    VALUES ('8f8f0f95-7408-44ad-a6f2-6f30f3e9a306', TIMESTAMPTZ '2026-04-06T09:13:47.393932+00:00', TRUE, '300 g', 149.0, '7f8f0f95-7408-44ad-a6f2-6f30f3e9a203', 142.0, NULL, 'SF-GC-300', TIMESTAMPTZ '2026-04-06T09:13:47.393932+00:00', '300 g');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260406091349_SeedLaunchCatalogAndOrderFlow') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260406091349_SeedLaunchCatalogAndOrderFlow', '9.0.1');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    ALTER TABLE subscriptions ADD "PaidAt" timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    ALTER TABLE subscriptions ADD "PaymentMethod" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    ALTER TABLE subscriptions ADD "PaymentReference" character varying(120);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    ALTER TABLE orders ADD "PaidAt" timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    ALTER TABLE orders ADD "PaymentReference" character varying(120);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    UPDATE delivery_zones SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.460461+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.460462+00:00'
    WHERE "Id" = '4f9f0f95-7408-44ad-a6f2-6f30f3e9a101';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    UPDATE delivery_zones SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.461423+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.461424+00:00'
    WHERE "Id" = '4f9f0f95-7408-44ad-a6f2-6f30f3e9a102';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    UPDATE delivery_zones SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.461426+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.461426+00:00'
    WHERE "Id" = '4f9f0f95-7408-44ad-a6f2-6f30f3e9a103';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.474751+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.474752+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a301';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.475261+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.475261+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a302';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.475263+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.475263+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a303';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.475264+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.475264+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a304';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.475266+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.475266+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a305';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.475267+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.475267+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a306';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    UPDATE products SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.471131+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.471132+00:00'
    WHERE "Id" = '7f8f0f95-7408-44ad-a6f2-6f30f3e9a201';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    UPDATE products SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.471395+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.471395+00:00'
    WHERE "Id" = '7f8f0f95-7408-44ad-a6f2-6f30f3e9a202';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    UPDATE products SET "CreatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.471396+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-07T04:00:01.471396+00:00'
    WHERE "Id" = '7f8f0f95-7408-44ad-a6f2-6f30f3e9a203';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407040003_AddPaymentTrackingAndAdminAccess') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260407040003_AddPaymentTrackingAndAdminAccess', '9.0.1');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    UPDATE delivery_zones SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '4f9f0f95-7408-44ad-a6f2-6f30f3e9a101';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    UPDATE delivery_zones SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '4f9f0f95-7408-44ad-a6f2-6f30f3e9a102';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    UPDATE delivery_zones SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '4f9f0f95-7408-44ad-a6f2-6f30f3e9a103';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a301';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a302';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a303';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a304';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a305';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a306';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    UPDATE products SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '7f8f0f95-7408-44ad-a6f2-6f30f3e9a201';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    UPDATE products SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '7f8f0f95-7408-44ad-a6f2-6f30f3e9a202';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    UPDATE products SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '7f8f0f95-7408-44ad-a6f2-6f30f3e9a203';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051445_FixSeedTimestampDeterminism') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260407051445_FixSeedTimestampDeterminism', '9.0.1');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    UPDATE delivery_zones SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '4f9f0f95-7408-44ad-a6f2-6f30f3e9a101';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    UPDATE delivery_zones SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '4f9f0f95-7408-44ad-a6f2-6f30f3e9a102';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    UPDATE delivery_zones SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '4f9f0f95-7408-44ad-a6f2-6f30f3e9a103';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a301';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a302';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a303';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a304';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a305';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    UPDATE product_variants SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '8f8f0f95-7408-44ad-a6f2-6f30f3e9a306';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    UPDATE products SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '7f8f0f95-7408-44ad-a6f2-6f30f3e9a201';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    UPDATE products SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '7f8f0f95-7408-44ad-a6f2-6f30f3e9a202';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    UPDATE products SET "CreatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00', "UpdatedAt" = TIMESTAMPTZ '2026-04-06T09:00:00+00:00'
    WHERE "Id" = '7f8f0f95-7408-44ad-a6f2-6f30f3e9a203';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407051623_InspectPendingModel') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260407051623_InspectPendingModel', '9.0.1');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407053332_AdminSessionsInvoicesAndCustomerSubscriptionActions') THEN
    ALTER TABLE orders ADD "CancellationReason" character varying(240);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407053332_AdminSessionsInvoicesAndCustomerSubscriptionActions') THEN
    ALTER TABLE orders ADD "InvoiceIssuedAt" timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407053332_AdminSessionsInvoicesAndCustomerSubscriptionActions') THEN
    ALTER TABLE orders ADD "InvoiceNumber" character varying(40);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407053332_AdminSessionsInvoicesAndCustomerSubscriptionActions') THEN
    ALTER TABLE admin_users ADD "PasswordHash" character varying(512);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407053332_AdminSessionsInvoicesAndCustomerSubscriptionActions') THEN
    CREATE TABLE admin_sessions (
        "Id" uuid NOT NULL,
        "AdminUserId" uuid NOT NULL,
        "TokenHash" character varying(128) NOT NULL,
        "ExpiresAt" timestamp with time zone NOT NULL,
        "RevokedAt" timestamp with time zone,
        "CreatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_admin_sessions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_admin_sessions_admin_users_AdminUserId" FOREIGN KEY ("AdminUserId") REFERENCES admin_users ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407053332_AdminSessionsInvoicesAndCustomerSubscriptionActions') THEN
    CREATE UNIQUE INDEX "IX_orders_InvoiceNumber" ON orders ("InvoiceNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407053332_AdminSessionsInvoicesAndCustomerSubscriptionActions') THEN
    CREATE INDEX "IX_admin_sessions_AdminUserId" ON admin_sessions ("AdminUserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407053332_AdminSessionsInvoicesAndCustomerSubscriptionActions') THEN
    CREATE UNIQUE INDEX "IX_admin_sessions_TokenHash" ON admin_sessions ("TokenHash");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260407053332_AdminSessionsInvoicesAndCustomerSubscriptionActions') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260407053332_AdminSessionsInvoicesAndCustomerSubscriptionActions', '9.0.1');
    END IF;
END $EF$;
COMMIT;

