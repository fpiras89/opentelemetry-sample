import { ModuleWithProviders, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { OpenTelemetryService } from "./open-telemetry.service";
import {
  OPEN_TELEMETRY_CONFIG,
  OpenTelemetryConfig,
} from "./open-telemetry.model";

@NgModule({
  imports: [CommonModule],
})
export class OpenTelemetryModule {
  constructor() {}

  static forRoot(
    config: OpenTelemetryConfig = {},
  ): ModuleWithProviders<OpenTelemetryModule> {
    return {
      ngModule: OpenTelemetryModule,
      providers: [
        OpenTelemetryService,
        {
          provide: OPEN_TELEMETRY_CONFIG,
          useValue: config,
        },
      ],
    };
  }
}
