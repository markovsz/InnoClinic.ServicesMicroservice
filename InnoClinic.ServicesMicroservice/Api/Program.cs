using Api.Extensions;
using Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureDb();
builder.Services.ConfigureRepositories();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.ConfigureServices();
builder.Services.ConfigureValidators();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
