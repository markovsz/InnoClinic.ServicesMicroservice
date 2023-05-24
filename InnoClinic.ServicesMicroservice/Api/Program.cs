using Api.Extensions;
using Api.Middlewares;
using Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureDb();
builder.Services.ConfigureRepositories();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.ConfigureMassTransit(builder.Configuration);
builder.Services.ConfigureServices();
builder.Services.ConfigureValidators();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.ConfigureSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionsHandler>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
