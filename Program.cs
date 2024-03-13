using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var livros = new List<Livros.API.Model.Livro>
{
    new Livros.API.Model.Livro{ Id = 1, Titulo = "Homo Sapiens - Uma breve história da humanidade", Paginas = 464, Ano = 2014, Autor = "Yuval Harari"},
    new Livros.API.Model.Livro{ Id = 2, Titulo = "Homo Deus: Uma breve hitória do amanhã", Paginas = 606, Ano = 2015, Autor = "Yuval Harari"},
    new Livros.API.Model.Livro{ Id = 3, Titulo = "21 lições para o século 21", Paginas = 432, Ano = 2018, Autor = "Yuval Harari"}
};

builder.Services.AddSingleton(livros);
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "Livros.API", Version = "v1"});
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/livros", () =>
{
    var taskService = app.Services.GetRequiredService<List<Livros.API.Model.Livro>>();
    return Results.Ok(taskService);
});

app.MapGet("/livros/{id}", (int id, HttpRequest request) =>
{
    var taskService = app.Services.GetRequiredService<List<Livros.API.Model.Livro>>();
    var task = taskService.FirstOrDefault(t => t.Id == id);

    if(task == null){
        return Results.NotFound();
    }

    return Results.Ok(taskService);
});

app.MapPost("/livros", (Livros.API.Model.Livro livro) => {
    var livroService = app.Services.GetRequiredService<List<Livros.API.Model.Livro>>();
    livro.Id = livroService.Max(t => t.Id) + 1;
    livroService.Add(livro);
    return Results.Created($"/livros/{livro.Id}", livro);
});


app.MapPut("/livros/{id}", (int id, Livros.API.Model.Livro livro) => {
    var livroService = app.Services.GetRequiredService<List<Livros.API.Model.Livro>>();
    var existingLivro = livroService.FirstOrDefault(t => t.Id == id);

    if(existingLivro == null){
        return Results.NotFound();
    }

    existingLivro.Titulo = livro.Titulo;
    existingLivro.Autor = livro.Autor;
    existingLivro.Ano = livro.Ano;
    existingLivro.Paginas = livro.Paginas;

    return Results.NoContent();
});

app.MapDelete("/livros/{id}", (int id) => {
    var livroService = app.Services.GetRequiredService<List<Livros.API.Model.Livro>>();
    var existingLivro = livroService.FirstOrDefault(t => t.Id == id);

    if(existingLivro == null){
        return Results.NotFound();
    }

    livroService.Remove(existingLivro);
    return Results.NoContent();

});
app.Run();

