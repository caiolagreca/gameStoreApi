using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games =
[
new (1, "Street Fight", "Fighting", 19.99M, new DateOnly(1992, 7, 5)),
new (2, "Final Fantasy", "Roleplaying", 59.99M, new DateOnly(2010, 9,30)),
new (3, "Fifa 2023", "Sports", 69.99M, new DateOnly(2022, 9, 27)),
];


// GET /games
app.MapGet("games", () => games);

// GET /games/id
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id)).WithName(GetGameEndpointName);

app.MapGet("games", (CreateGameDto newGame) =>
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

app.Run();
