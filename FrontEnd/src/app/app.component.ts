import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { RouterOutlet } from "@angular/router";
import { MatSnackBar, MatSnackBarModule } from "@angular/material/snack-bar";
import { OpenTelemetryModule } from "./shared/modules/open-telemetry/open-telemetry.module";
import { BehaviorSubject } from "rxjs";
import { OpenTelemetryService } from "./shared/modules/open-telemetry/open-telemetry.service";

export interface ToDo {
  id: string;
  text: string;
  done: boolean;
}

@Component({
  selector: "app-root",
  standalone: true,
  imports: [CommonModule, HttpClientModule, RouterOutlet, MatSnackBarModule],
  templateUrl: "./app.component.html",
  styles: [],
})
export class AppComponent implements OnInit {
  title = "fe";
  todosSubject = new BehaviorSubject<ToDo[]>([]);
  todos$ = this.todosSubject.asObservable();

  constructor(
    private http: HttpClient,
    private snackBar: MatSnackBar,
    private otel: OpenTelemetryService,
  ) {}

  ngOnInit(): void {
    this.otel
      .withSpan("init", this.http.get<ToDo[]>("http://localhost:5000/api/todo"))
      .subscribe({
        next: (data) => {
          this.todosSubject.next(data);
        },
        error: (error: any) => {
          console.log(error);
          this.snackBar.open(error.message, "OK", {
            duration: 5 * 1000,
          });
        },
      });
  }

  addTodo(text: string) {
    this.otel
      .withSpan(
        "add",
        this.http.post<ToDo>("http://localhost:5000/api/todo", {
          text,
        }),
      )
      .subscribe({
        next: (todo) => {
          this.todosSubject.next([...this.todosSubject.value, todo]);
        },
        error: (error: any) => {
          console.log(error);
          this.snackBar.open(error.message, "OK", {
            duration: 5 * 1000,
          });
        },
      });
  }

  removeTodo(todo: ToDo) {
    this.otel
      .withSpan(
        "remove",
        this.http.delete<boolean>(`http://localhost:5000/api/todo/${todo.id}`),
      )
      .subscribe({
        next: (result) => {
          const list = this.todosSubject.value;
          const deleteTodoIndex = list.findIndex((e) => e.id == todo.id);
          if (deleteTodoIndex >= 0) {
            list.splice(deleteTodoIndex, 1);
            this.todosSubject.next(list);
          }
        },
        error: (error: any) => {
          console.log(error);
          this.snackBar.open(error.message, "OK", {
            duration: 5 * 1000,
          });
        },
      });
  }

  updateTodo(todo: ToDo) {
    this.otel
      .withSpan(
        "update",
        this.http.put<ToDo>("http://localhost:5000/api/todo", {
          ...todo,
          done: true,
        }),
      )
      .subscribe({
        next: (todo) => {
          const list = this.todosSubject.value;
          const updatedTodo = list.find((e) => e.id == todo.id);
          if (updatedTodo) {
            updatedTodo.done = true;
          }
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
