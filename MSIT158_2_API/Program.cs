using Microsoft.EntityFrameworkCore;
using MSIT158_2_API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//////////////////////////////////////////////////////////////////////////////////
//builder.Services.AddSession();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
//////////////////////////////////////////////////////////////////////////////////
builder.Services.AddDbContext<SelectShopContext>(
options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("SelectShopConnection")
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//////////////////////////////////////////////////////////////////////////////////
app.UseCors("AllowAll");
//app.UseSession();
//////////////////////////////////////////////////////////////////////////////////

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
