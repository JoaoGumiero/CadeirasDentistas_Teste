using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CadeirasDentistas.midleware;
using Microsoft.AspNetCore.SignalR;
using CadeirasDentistas.Repository;
using CadeirasDentistas.services;
using CadeirasDentistas.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//Swagger
builder.Services.AddSwaggerGen();

// Configuração de dependências
builder.Services.AddScoped<ICadeiraRepository, CadeiraRepository>();
builder.Services.AddScoped<ICadeiraService, CadeiraService>();
builder.Services.AddScoped<IAlocacaoRepository, AlocacaoRepository>();
builder.Services.AddScoped<IAlocacaoService, AlocacaoService>();

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

// Configurar conexão com o banco de dados (Connection String)
builder.Services.AddSingleton(new ApplicationDbContext(connectionString));


var app = builder.Build();

// Configuração do pipeline de requisição
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TesteCadeirasDentistasJGumiero V1");
        options.RoutePrefix = string.Empty; // Swagger acessível (http://localhost:8080)
    });
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseDeveloperExceptionPage();
//app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Middleware de tratamento de exceções
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
