import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { RouterOutlet } from "@angular/router";
import { MatSnackBar, MatSnackBarModule } from "@angular/material/snack-bar";
import { OpenTelemetryModule } from "./shared/modules/open-telemetry/open-telemetry.module";
import { BehaviorSubject } from "rxjs";

@Component({
  selector: "app-root",
  standalone: true,
  imports: [CommonModule, HttpClientModule, RouterOutlet, MatSnackBarModule],
  templateUrl: "./app.component.html",
  styles: [],
})
export class AppComponent {
  title = "fe";
  todosSubject = new BehaviorSubject<{ text: string; done: boolean }[]>([]);
  todos$ = this.todosSubject.asObservable();

  constructor(private http: HttpClient, private snackBar: MatSnackBar) {}

  addTodo(newTodo: string) {
    this.http.get("http://localhost:5000").subscribe({
      next: () => {
        const list = this.todosSubject.value;
        this.todosSubject.next([...list, { text: newTodo, done: false }]);
      },
      error: (error: any) => {
        console.log(error);
        this.snackBar.open(error.message, "OK", {
          duration: 5 * 1000,
        });
      },
    });
  }
}
