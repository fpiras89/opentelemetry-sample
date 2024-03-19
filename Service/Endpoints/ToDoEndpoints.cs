using Examples.Service.Entities;
using Examples.Service.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Examples.Service.Endpoints;

public static class ToDoEndpoints
{
    public static IEndpointRouteBuilder MapToDoApi(this IEndpointRouteBuilder builder, string prefix = "/api/todo")
    {
        builder.MapGet($"{prefix.TrimEnd('/')}", async Task<IResult>() => 
        {
            return Results.Ok(ToDoRepository.GetAll());
        });

        builder.MapGet($"{prefix.TrimEnd('/')}/{{id}}", async Task<IResult> ([FromRoute]Guid id) =>
        {
            return Results.Ok(ToDoRepository.Get(id));
        });

        builder.MapPost($"{prefix.TrimEnd('/')}", async Task<IResult> ([FromBody]ToDo todo) =>
        {
            return Results.Ok(ToDoRepository.Add(todo));
        });

        builder.MapPut($"{prefix.TrimEnd('/')}", async Task<IResult> ([FromBody] ToDo todo) =>
        {
            return Results.Ok(ToDoRepository.Update(todo));
        });

        builder.MapDelete($"{prefix.TrimEnd('/')}/{{id}}", async Task<IResult> ([FromRoute] Guid id) =>
        {
            return Results.Ok(ToDoRepository.Delete(id));
        });

        return builder;
    }
}