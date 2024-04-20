using Examples.Service.Application.Dtos;
using Examples.Service.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Examples.Service.Presentation.Endpoints;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapToDoApi(this IEndpointRouteBuilder builder, string prefix = "/api/todo")
    {
        builder.MapGet($"{prefix.TrimEnd('/')}", async Task<IResult> (ITodoService todoService) =>
        {
            return Results.Ok(await todoService.GetAllAsync());
        });

        builder.MapGet($"{prefix.TrimEnd('/')}/{{id}}", async Task<IResult> ([FromRoute] Guid id, ITodoService todoService) =>
        {
            return Results.Ok(await todoService.GetAsync(id));
        });

        builder.MapPost($"{prefix.TrimEnd('/')}", async Task<IResult> ([FromBody] TodoDto todo, ITodoService todoService) =>
        {
            return Results.Ok(await todoService.AddAsync(todo));
        });

        builder.MapPut($"{prefix.TrimEnd('/')}", async Task<IResult> ([FromBody] TodoDto todo, ITodoService todoService) =>
        {
            return Results.Ok(await todoService.UpdateAsync(todo));
        });

        builder.MapDelete($"{prefix.TrimEnd('/')}/{{id}}", async Task<IResult> ([FromRoute] Guid id, ITodoService todoService) =>
        {
            return Results.Ok(await todoService.DeleteAsync(id));
        });

        return builder;
    }
}