using AspNetCore_Practising.Models;
using AspNetCore_Practising.Models.Interfaces;
using AspNetCore_Practising.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ITaskService>(new InMemoryTaskService());

var app = builder.Build();
var todos = new List<Todo_Model>();

app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todos/$1"));
app.UseRewriter(new RewriteOptions().AddRedirect("tasks", "todos"));

app.Use(async (context, next) =>
{
    Console.WriteLine($"\n[{context.Request.Method} on {context.Request.Path} at {DateTime.Now}] Started");
    await next(context);
    Console.WriteLine($"[{context.Request.Method} on {context.Request.Path} at {DateTime.Now}] Ended");
});

app.MapGet("/", () => "Hello World!");

// todos
// get
app.MapGet("/todos", (ITaskService service) => service.GetTodos());
app.MapGet("/todos/{id}", Results<Ok<Todo_Model>, NotFound> (int id, ITaskService service) =>
{
    var record = service.GetTodoById(id);

    return record == null
    ? TypedResults.NotFound()
    : TypedResults.Ok(record);
});

// post
app.MapPost("/todos", (Todo_Model todo, ITaskService service) =>
{
    service.AddTodo(todo);
    return TypedResults.Created("/todos/{id}", todo);
}).AddEndpointFilter(async (context, next) =>
{
    var argument = context.GetArgument<Todo_Model>(0);
    var errors = new Dictionary<string, string[]>();

    if (string.IsNullOrWhiteSpace(argument.Name))
        errors.Add(nameof(argument.Name), ["Todo's name cannot be null or empty"]);

    if (argument.DueDate < DateTime.Now)
        errors.Add(nameof(argument.DueDate), ["You cannot add a todo with due date in the past"]);

    if (argument.IsCompleted)
        errors.Add(nameof(argument.IsCompleted), ["You cannot add a completed todo"]);

    if (errors.Count > 0)
        return Results.ValidationProblem(errors);

    return await next(context);
});

// delete
app.MapDelete("/todos/{id}", (int id, ITaskService service) =>
{
    service.DeleteTodoById(id);
    return TypedResults.NotFound();
});

app.Run();