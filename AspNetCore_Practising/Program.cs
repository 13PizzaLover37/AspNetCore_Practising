using AspNetCore_Practising.Models;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var todos = new List<Todo_Model>();

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

app.Run();
