import { InjectionToken } from "@angular/core";
import {
  SDKRegistrationConfig,
  SpanProcessor,
  TracerConfig,
} from "@opentelemetry/sdk-trace-web";
import { Instrumentation } from "@opentelemetry/instrumentation";

export interface OpenTelemetryConfig {
  tracerConfig?: TracerConfig | undefined;
  spanProcessors?: SpanProcessor[] | undefined | null;
  sdkRegistrationConfig?: SDKRegistrationConfig | undefined;
  instrumentations?: Instrumentation[];
}

export const OPEN_TELEMETRY_CONFIG = new InjectionToken<string>(
  "OPEN_TELEMETRY_CONFIG",
);
