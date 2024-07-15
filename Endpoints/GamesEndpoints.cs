using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games =
    [
    new (1, "Street Fight", "Fighting", 19.99M, new DateOnly(1992, 7, 5)),
    new (2, "Final Fantasy", "Roleplaying", 59.99M, new DateOnly(2010, 9,30)),
    new (3, "Fifa 2023", "Sports", 69.99M, new DateOnly(2022, 9, 27)),
];

    //Extension method (this keyword)
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/id
        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
            .WithName(GetGameEndpointName);

        // POST
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            games.Add(game);

            // Retorna uma resposta CreatedAtRoute com o local do novo recurso
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        //PUT /games/id
        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
        {

            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.ReleaseDate
            );

            return Results.NoContent();
        });

        //DELETE /games/id
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent(); ;
        });

        return group;
    }
}
