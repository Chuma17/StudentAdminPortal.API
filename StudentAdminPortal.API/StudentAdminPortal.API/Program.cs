using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.API.Models;
using StudentAdminPortal.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
    
builder.Services.AddControllers();

builder.Services.AddDbContext<StudentAdminContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("StudentAdminPortalDB")));

builder.Services.AddScoped<IStudentRepository, SqlStudentRepository>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Automapper Injection
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddCors((options) =>
{
    options.AddPolicy("angularApplication", builder =>
    {
        builder.WithOrigins("http://localhost:4200/")
        .AllowAnyHeader()
        .WithMethods("GET", "POST", "PUT", "DELETE")
        .WithExposedHeaders("*")
        .AllowAnyOrigin();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("angularApplication");

app.UseAuthorization();

app.MapControllers();

app.Run();
