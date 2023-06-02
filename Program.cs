using ExoAPI.Context;
using  ExoAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.DependencyInjection.IServiceCollection;

var builder = WebApplication.CreateBuilder(args);
//The sample app registers the IMyDependency service with the concrete type MyDependency. The AddScoped method registers the service with a scoped lifetime, the lifetime of a single request. Service lifetimes are described later in this topic.

builder.Services.AddScoped<IStudentRepository, StudentRepository>();

//Connection string DB Sqlite:
var connectionString = builder.Configuration.GetConnectionString("Students") ?? "Data Source=Students.db";
// Add services to the container.
builder.Services.AddDbContext<ContextDB>(
    opts => opts.UseSqlite(connectionString)
);

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
