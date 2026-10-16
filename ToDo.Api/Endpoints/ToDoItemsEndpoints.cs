using Microsoft.EntityFrameworkCore;
using ToDo.Api.Data;
using ToDo.Api.DTOs;
using ToDo.Api.Models;

namespace ToDo.Api.Endpoints
{
    public static class ToDoItemsEndpoints
    {
        public static void MapToDoItemsEndpoint(this IEndpointRouteBuilder routes)
        {
            var toDoItemsEndpints = routes.MapGroup("/pendientes").WithTags("Pendientes");


            toDoItemsEndpints.MapGet("/", GetAllToDoItems);
            toDoItemsEndpints.MapGet("/{id:int}", GetToDoItemById);
            toDoItemsEndpints.MapPost("/", CreateToDoItem);
            toDoItemsEndpints.MapPut("/{id:int}", UpdateToDoItem);
            toDoItemsEndpints.MapDelete("/{id:int}", DeleteToDoItem);


        }

        private static async Task<IResult> GetAllToDoItems
         (AppDbContext dbContext)
        {
            var toDoItemsDtos = await dbContext.ToDoItems
                .Select(p => new ToDoItemDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsCompleted = p.IsCompleted,
                }).ToListAsync();


            return Results.Ok(toDoItemsDtos);
        }

        private static async Task<IResult> GetToDoItemById(AppDbContext dbContext, int id)
        {
            var toDoItem = await dbContext.ToDoItems.FirstOrDefaultAsync(tdi => tdi.Id == id);
            if (toDoItem is null)
            {
                return Results.NotFound();
            }
            var toDoItemDto = new ToDoItemDto()
            {
                Id = toDoItem.Id,
                Name = toDoItem.Name,
                IsCompleted = toDoItem.IsCompleted
            };
            return Results.Ok(toDoItemDto);
        }

        private static async Task<IResult> CreateToDoItem(
          AppDbContext dbContext, CreateToDoItemDto createToDoItemDto)
        {
            var toDoItem = new ToDoItem()
            {
                Name = createToDoItemDto.Name,
                IsCompleted = false
            };

            await dbContext.ToDoItems.AddAsync(toDoItem);
            await dbContext.SaveChangesAsync();

            var toDoItemDto = new ToDoItemDto()
            {
                Id = toDoItem.Id,
                Name = toDoItem.Name,
                IsCompleted = toDoItem.IsCompleted
            };

            return Results.Created($"/pendientes/{toDoItem.Id}", toDoItemDto);
        }


        private static async Task<IResult> UpdateToDoItem(
            AppDbContext dbContext, int id, UpdateToDoItemDto item)
        {
            var toDoItem = await dbContext.ToDoItems
                .FirstOrDefaultAsync(tdi => tdi.Id == id);

            if (toDoItem is null)
            {
                return Results.NotFound();
            }

            toDoItem.Name = item.Name;
            toDoItem.IsCompleted = item.IsCompleted;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }


        private static async Task<IResult> DeleteToDoItem
                 (AppDbContext dbContext, int id)
        {
            var toDoItem = await dbContext.ToDoItems.FirstOrDefaultAsync(tdi => tdi.Id == id);
            if (toDoItem is null)
            {
                return Results.NotFound();
            }
            dbContext.ToDoItems.Remove(toDoItem);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        }



    }
}

