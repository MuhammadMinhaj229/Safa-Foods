START TRANSACTION;
CREATE TABLE integration_webhook_events (
    "Id" uuid NOT NULL,
    "Provider" character varying(50) NOT NULL,
    "EventType" character varying(120) NOT NULL,
    "ExternalEventId" character varying(120),
    "PayloadJson" text NOT NULL,
    "ProcessingStatus" character varying(40) NOT NULL,
    "ReceivedAt" timestamp with time zone NOT NULL,
    "ProcessedAt" timestamp with time zone,
    CONSTRAINT "PK_integration_webhook_events" PRIMARY KEY ("Id")
);

CREATE INDEX "IX_integration_webhook_events_ExternalEventId" ON integration_webhook_events ("ExternalEventId");

CREATE INDEX "IX_integration_webhook_events_Provider_ReceivedAt" ON integration_webhook_events ("Provider", "ReceivedAt");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260407064845_MetaWebhookEventStore', '9.0.1');

COMMIT;

