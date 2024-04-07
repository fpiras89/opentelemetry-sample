using Examples.Service.Domain.Entities;
using Examples.Service.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Examples.Service.Presentation.Endpoints;

public static class ToDoEndpoints
{
    public static IEndpointRouteBuilder MapToDoApi(this IEndpointRouteBuilder builder, string prefix = "/api/todo")
    {
        builder.MapGet($"{prefix.TrimEnd('/')}", async Task<IResult> (ToDoRepository repository) =>
        {
            return Results.Ok(repository.GetAll());
        });

        builder.MapGet($"{prefix.TrimEnd('/')}/{{id}}", async Task<IResult> ([FromRoute] Guid id, ToDoRepository repository) =>
        {
            return Results.Ok(repository.Get(id));
        });

        builder.MapPost($"{prefix.TrimEnd('/')}", async Task<IResult> ([FromBody] ToDo todo, ToDoRepository repository) =>
        {
            return Results.Ok(repository.Add(todo));
        });

        builder.MapPut($"{prefix.TrimEnd('/')}", async Task<IResult> ([FromBody] ToDo todo, ToDoRepository repository) =>
        {
            return Results.Ok(repository.Update(todo));
        });

        builder.MapDelete($"{prefix.TrimEnd('/')}/{{id}}", async Task<IResult> ([FromRoute] Guid id, ToDoRepository repository) =>
        {
            return Results.Ok(repository.Delete(id));
        });

        return builder;
    }
}