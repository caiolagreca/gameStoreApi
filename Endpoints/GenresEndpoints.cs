using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    const string GetGenreEndpointName = "GetGenre";

    //Extension method (this keyword)
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("genres").WithParameterValidation();

        // GET /Genres
        group.MapGet("/", async (GameStoreContext dbContext) =>

       await dbContext.Genres.Select(genre => genre.ToGenreDetailsDto()).AsNoTracking().ToListAsync());

        // GET /genres/id
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Genre? genre = await dbContext.Genres.FindAsync(id);
            return genre is null ? Results.NotFound() : Results.Ok(genre.ToGenreDetailsDto());
        })
            .WithName(GetGenreEndpointName);

        // POST
        group.MapPost("/", async (CreateGenreDto newGenre, GameStoreContext dbContext) =>
                {
                    Genre genre = newGenre.ToEntity();

                    dbContext.Genres.Add(genre);
                    await dbContext.SaveChangesAsync();

                    // Retorna uma resposta CreatedAtRoute com o local do novo recurso
                    return Results.CreatedAtRoute(GetGenreEndpointName, new { id = genre.Id }, genre.ToGenreDetailsDto());
                });

        //PUT /genres/id
        group.MapPut("/{id}", async (int id, UpdateGenreDto updateGenre, GameStoreContext dbContext) =>
        {

            var existingGenre = await dbContext.Genres.FindAsync(id);

            if (existingGenre is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGenre).CurrentValues.SetValues(updateGenre.ToEntity(id));
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        //DELETE /Genres/id
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Genres.Where(genre => genre.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });

        return group;
    }
}
