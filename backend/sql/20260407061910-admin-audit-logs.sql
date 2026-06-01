START TRANSACTION;
CREATE TABLE admin_audit_logs (
    "Id" uuid NOT NULL,
    "AdminUserId" uuid,
    "Role" integer NOT NULL,
    "Action" character varying(100) NOT NULL,
    "EntityType" character varying(80) NOT NULL,
    "EntityId" uuid,
    "MetadataJson" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_admin_audit_logs" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_admin_audit_logs_admin_users_AdminUserId" FOREIGN KEY ("AdminUserId") REFERENCES admin_users ("Id") ON DELETE SET NULL
);

CREATE INDEX "IX_admin_audit_logs_AdminUserId" ON admin_audit_logs ("AdminUserId");

CREATE INDEX "IX_admin_audit_logs_CreatedAt" ON admin_audit_logs ("CreatedAt");

CREATE INDEX "IX_admin_audit_logs_EntityType_EntityId" ON admin_audit_logs ("EntityType", "EntityId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260407061910_AdminAuditLogs', '9.0.1');

COMMIT;

