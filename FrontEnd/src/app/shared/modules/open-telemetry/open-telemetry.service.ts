import { Inject, Injectable } from "@angular/core";
import { Span, WebTracerProvider } from "@opentelemetry/sdk-trace-web";
import { registerInstrumentations } from "@opentelemetry/instrumentation";
import {
  OPEN_TELEMETRY_CONFIG,
  OpenTelemetryConfig,
} from "./open-telemetry.model";
import { Tracer, context, trace } from "@opentelemetry/api";
import { Observable, Subscription, from } from "rxjs";

@Injectable()
export class OpenTelemetryService {
  public provider: WebTracerProvider;

  public get activeContext() {
    return context.active();
  }

  public get context() {
    return context;
  }

  public get trace() {
    return trace;
  }

  constructor(@Inject(OPEN_TELEMETRY_CONFIG) config: OpenTelemetryConfig) {
    // Init tracer provider
    this.provider = new WebTracerProvider(config.tracerConfig);

    // Register span processors
    if (config.spanProcessors) {
      config.spanProcessors.forEach((sp) => this.provider.addSpanProcessor(sp));
    }

    // Register SDK configuration
    this.provider.register(config.sdkRegistrationConfig);

    // Registering instrumentations
    registerInstrumentations({
      instrumentations: config.instrumentations || [],
    });
  }

  getTracer(
    name?: string,
    version?: string | undefined,
    options?: any,
  ): Tracer {
    return this.provider.getTracer(name || "default", version, options);
  }

  withContext(fn: () => void, span: Span) {
    this.context.with(this.trace.setSpan(this.activeContext, span), fn);
  }

  withSpan<T>(name: string, src: Promise<T> | Observable<T>): Observable<T> {
    return new Observable<T>((observable) => {
      let subscription: Subscription;
      const singleSpan = this.getTracer().startSpan(name);
      singleSpan.addEvent("start");
      context.with(trace.setSpan(this.activeContext, singleSpan), () => {
        subscription = from(src).subscribe({
          next: (data) => {
            singleSpan.addEvent("next");
            observable.next(data);
          },
          complete: () => {
            singleSpan.addEvent("completed");
            singleSpan.end();
            observable.complete();
          },
          error: (err?: any) => {
            singleSpan.addEvent("error");
            observable.error(err);
          },
        });
      });
      return () => subscription?.unsubscribe();
    });
  }
}
