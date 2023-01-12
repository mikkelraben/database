using database.Models;
using database.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<TodoDatabaseSettings>(builder.Configuration.GetSection(nameof(TodoDatabaseSettings)));

builder.Services.AddSingleton<TodosService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "allow dev", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://lively-wave-0723cd803.2.azurestaticapps.net").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("allow dev");

app.MapControllers();

app.Run();
