var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


using MinhaBiblioteca.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuração dos Controllers
builder.Services.AddControllers();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registro do Repository como Singleton
builder.Services.AddSingleton<IMaterialRepository, MaterialRepository>();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Configurações da aplicação
app.UseHttpsRedirection();
app.UseCors("PermitirFrontend");
app.UseAuthorization();

app.MapControllers();

app.Run();

