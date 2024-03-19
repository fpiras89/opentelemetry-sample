import { ApplicationConfig, importProvidersFrom } from "@angular/core";
import { provideRouter } from "@angular/router";

import { routes } from "./app.routes";
import { provideAnimations } from "@angular/platform-browser/animations";
import { OpenTelemetryModule } from "./shared/modules/open-telemetry/open-telemetry.module";
import { BatchSpanProcessor } from "@opentelemetry/sdk-trace-web";
import { OTLPTraceExporter } from "@opentelemetry/exporter-trace-otlp-http";
import { ZoneContextManager } from "@opentelemetry/context-zone-peer-dep";
import { B3Propagator } from "@opentelemetry/propagator-b3";

import { DocumentLoadInstrumentation } from "@opentelemetry/instrumentation-document-load";
import { UserInteractionInstrumentation } from "@opentelemetry/instrumentation-user-interaction";
import { XMLHttpRequestInstrumentation } from "@opentelemetry/instrumentation-xml-http-request";
import { FetchInstrumentation } from "@opentelemetry/instrumentation-fetch";

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimations(),
    importProvidersFrom(
      OpenTelemetryModule.forRoot({
        tracerConfig: {
          resource: {
            merge: (other) => other as any,
            attributes: {
              "service.name": "angular",
            },
          },
        },
        spanProcessors: [
          new BatchSpanProcessor(
            new OTLPTraceExporter({
              url: "http://localhost:4318/v1/traces",
              headers: {}, // an optional object containing custom headers to be sent with each request
              concurrencyLimit: 10, // an optional limit on pending requests
            }),
            {
              // The maximum queue size. After the size is reached spans are dropped.
              maxQueueSize: 100,
              // The maximum batch size of every export. It must be smaller or equal to maxQueueSize.
              maxExportBatchSize: 10,
              // The interval between two consecutive exports
              scheduledDelayMillis: 500,
              // How long the export can run before it is cancelled
              exportTimeoutMillis: 30000,
            },
          ),
        ],
        sdkRegistrationConfig: {
          // Changing default contextManager to use ZoneContextManager - supports asynchronous operations - optional
          contextManager: new ZoneContextManager(),
          propagator: new B3Propagator(),
        },
        instrumentations: [
          new DocumentLoadInstrumentation(),
          new UserInteractionInstrumentation(),
          new XMLHttpRequestInstrumentation(),
          new FetchInstrumentation(),
        ],
      }),
    ),
  ],
};
