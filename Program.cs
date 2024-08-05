using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NehuenOrganico.Data;
using NehuenOrganico.Models;
using NehuenOrganico.Repositories;
using NehuenOrganico.Repositories.Interfaces;
using NehuenOrganico.Services;

var builder = WebApplication.CreateBuilder(args);


SQLitePCL.Batteries.Init();

var connectionString = builder.Configuration.GetConnectionString("default");

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlite(connectionString));

builder.Services.AddIdentity<AppUser, IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
    


builder.Services.AddScoped<IProductRepository, ProductsRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoriesRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IDashboardAdmRepository, DashboardAdmRepository>();

builder.Services.AddScoped<OrderService>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();



app.Run();
