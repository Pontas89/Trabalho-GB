
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

// Ativação do Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Configuração da aplicação
app.UseHttpsRedirection();
app.UseCors("PermitirFrontend");
app.UseAuthorization();

app.MapControllers();

app.Run();
