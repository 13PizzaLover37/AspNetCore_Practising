using AspNetCore_Practising.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
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
app.MapGet("/todos", () => todos);
app.MapGet("/todos/{id}", Results<Ok<Todo_Model>, NotFound> (int id) =>
{
    var record = todos.FirstOrDefault(el => el.Id == id);

    return record == null
    ? TypedResults.NotFound()
    : TypedResults.Ok(record);
});

// post
app.MapPost("/todos", (Todo_Model todo) =>
{
    todos.Add(todo);
    return TypedResults.Created("/todos/{id}", todo);
});

// delete
app.MapDelete("/todos/{id}", (int id) => { todos.RemoveAll(el => el.Id == id); return TypedResults.NotFound(); });

app.Run();
