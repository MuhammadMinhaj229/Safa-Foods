START TRANSACTION;
ALTER TABLE orders ADD "CancellationReason" character varying(240);

ALTER TABLE orders ADD "InvoiceIssuedAt" timestamp with time zone;

ALTER TABLE orders ADD "InvoiceNumber" character varying(40);

ALTER TABLE admin_users ADD "PasswordHash" character varying(512);

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

CREATE UNIQUE INDEX "IX_orders_InvoiceNumber" ON orders ("InvoiceNumber");

CREATE INDEX "IX_admin_sessions_AdminUserId" ON admin_sessions ("AdminUserId");

CREATE UNIQUE INDEX "IX_admin_sessions_TokenHash" ON admin_sessions ("TokenHash");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260407053332_AdminSessionsInvoicesAndCustomerSubscriptionActions', '9.0.1');

COMMIT;

