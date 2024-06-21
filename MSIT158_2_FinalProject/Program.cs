using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;

var builder = WebApplication.CreateBuilder(args);


/*
Scaffold-DbContext   "Data Source=.;Initial Catalog=SelectShop;Integrated Security=True;Encrypt=True;Trust Server Certificate=True" Microsoft.EntityFrameworkCore.SqlServer      -OutputDir       Models -force
*///��s�ҫ�(�ƻs�W�����)
builder.Services.AddDbContext<SelectShopContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("SelectShopConnection")
));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
//�s�WSession
builder.Services.AddSwaggerGen();
//�s�WSwagger
//////////////////////////////////////////////////////////////////////////////////
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
//////////////////////////////////////////////////////////////////////////////////
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
//�ҥ�Session
app.UseSwagger();
app.UseSwaggerUI();
//�ҥ�Swagger
//////////////////////////////////////////////////////////////////////////////////
app.UseCors("AllowAll");
//////////////////////////////////////////////////////////////////////////////////
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=Search}/{id?}");

app.Run();
