using AutoMapper;
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
         (AppDbContext dbContext,
            IMapper mapper)
        {
            var toDoItemsDtos = await dbContext.ToDoItems
                .Select(p => mapper.Map<ToDoItemDto>(p))
                .ToListAsync();
                //{
                //    Id = p.Id,
                //    Name = p.Name,
                //    IsCompleted = p.IsCompleted,
                //}).ToListAsync();

            return Results.Ok(toDoItemsDtos);
        }

        private static async Task<IResult> GetToDoItemById(
            int id,
            AppDbContext dbContext,
            IMapper mapper)
        {
            var toDoItem = await dbContext.ToDoItems.FirstOrDefaultAsync(tdi => tdi.Id == id);
            if (toDoItem is null)
            {
                return Results.NotFound();
            }

            var toDoItemDto= mapper.Map<ToDoItemDto>(toDoItem);
            return Results.Ok(toDoItemDto);
        }

        private static async Task<IResult> CreateToDoItem(
          CreateToDoItemDto createToDoItemDto,
          AppDbContext dbContext, 
          IMapper mapper)
        {
            var toDoItem= mapper.Map<ToDoItem>(createToDoItemDto);
            toDoItem.IsCompleted = false;
            await dbContext.ToDoItems.AddAsync(toDoItem);
            await dbContext.SaveChangesAsync();

            var toDoItemDto = mapper.Map<ToDoItemDto>(toDoItem);

            return Results.Created($"/pendientes/{toDoItem.Id}", toDoItemDto);
        }


        private static async Task<IResult> UpdateToDoItem(
            int id, 
            AppDbContext dbContext, 
            UpdateToDoItemDto item,
            IMapper mapper)
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
            (int id,
            AppDbContext dbContext)
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

