

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

using Employee.api.Model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ad services to the container

builder.Services.AddControllers();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("enabledAll", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
builder.Services.AddDbContext<EmployeeDbContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("empCon")));

var app = builder.Build();

// Configure the HTTP request pipeline

app.UseHttpsRedirection();
app.UseCors("enabledAll");
app.UseAuthorization();
app.MapControllers();

app.Run();