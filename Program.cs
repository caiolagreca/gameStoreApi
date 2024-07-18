using GameStore.Api;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;

//Criação do WebApplicationBuilder: Inicializa a configuração da aplicação.
var builder = WebApplication.CreateBuilder(args);

//Definição da String de Conexão: Especifica o arquivo do banco de dados SQLite a ser usado.
var connString = builder.Configuration.GetConnectionString("GameStore");

//Registro do Contexto do Banco de Dados: Configura o Entity Framework para usar o SQLite e registra o GameStoreContext no contêiner de dependências.
builder.Services.AddSqlite<GameStoreContext>(connString);

//Construção e Execução da Aplicação: Constrói a aplicação e começa a escutar as requisições HTTP.
var app = builder.Build();

//Mapeamento dos Endpoints: Define os endpoints da API que interagem com o banco de dados usando o GameStoreContext.
app.MapGamesEndpoints();

// Aplica migrações pendentes ao banco de dados.
app.MigrateDb();

app.Run();
