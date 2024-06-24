using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;

var builder = WebApplication.CreateBuilder(args);


/*
Scaffold-DbContext   "Data Source=.;Initial Catalog=SelectShop;Integrated Security=True;Encrypt=True;Trust Server Certificate=True" Microsoft.EntityFrameworkCore.SqlServer      -OutputDir       Models -force
*///更新模型(複製上面整行)
builder.Services.AddDbContext<SelectShopContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("SelectShopConnection")
));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
//新增Session
builder.Services.AddSwaggerGen();
//新增Swagger
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
//啟用Session
app.UseSwagger();
app.UseSwaggerUI();
//啟用Swagger
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
