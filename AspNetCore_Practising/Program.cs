using AspNetCore_Practising.Models;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
IEnumerable<Todo_Model> todos = new List<Todo_Model>();

app.MapGet("/", () => "Hello World!");

// todos
app.MapGet("/todos", () => todos);
app.MapGet("/todos/{id}", Results<Ok<Todo_Model>, NotFound> (int id) =>
{
    var record = todos.FirstOrDefault(el => el.Id == id);

    return record == null 
    ? TypedResults.NotFound()
    : TypedResults.Ok(record);
});

app.Run();
