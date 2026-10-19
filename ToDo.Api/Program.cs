using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ToDo.Api.Models;
using ToDo.Api.Data;
using ToDo.Api.Endpoints;
using ToDo.Api.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Agrega el servicio para la conexion a la base de datos SQL Server 
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<CreateToDoItemDto, ToDoItem>();
    cfg.CreateMap<ToDoItem, ToDoItemDto>();
    cfg.CreateMap<CreateToDoItemDto, ToDoItem>();

});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


 //Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
   app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapToDoItemsEndpoint();


//app.MapGet("/api/pendientes",
//    async (AppDbContext dbContext) =>
//    {
//        var toDoItems = await dbContext.ToDoItems.ToListAsync();
//        return Results.Ok(toDoItems);
//    });

//app.MapGet("/api/pendientes/{id:int}",
//    async (AppDbContext dbContext, int id) =>
//    {
//        var toDoItem = await dbContext.ToDoItems.FirstOrDefaultAsync(tdi => tdi.Id == id);
//        if (toDoItem is null)
//        {
//            return Results.NotFound();
//        }
//        return Results.Ok(toDoItem);
//    });
//app.MapPost("/api/pendientes",
//    async (AppDbContext dbContext, ToDoItem toDoItem) =>
//    {
//        toDoItem.IsCompleted = false;
//        await dbContext.ToDoItems.AddAsync(toDoItem);
//        await dbContext.SaveChangesAsync();
//        return Results.Created($"/api/pendientes/{toDoItem.Id}", toDoItem);
//    }
//    );

app.Run();