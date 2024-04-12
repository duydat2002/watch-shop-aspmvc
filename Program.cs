using Microsoft.EntityFrameworkCore;
using WatchShop2.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromDays(1));
builder.Services.AddControllersWithViews();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddDbContext<WatchShop2Context>(options =>
{
    string connectstring = builder.Configuration.GetConnectionString("DuyDat");
    options.UseSqlServer(connectstring);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();


app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    var isAdminPath = context.Request.Path.StartsWithSegments("/admin");
    var isApiPath = context.Request.Path.StartsWithSegments("/api");
    // RoleId = 2 = Customer
    var RoleId = context.Session.GetInt32("RoleId");
    // Console.WriteLine("path: " + path);
    // Console.WriteLine("RoleId: " + RoleId);
    // Console.WriteLine("isAdminPath: " + isAdminPath);

    if (isApiPath)
    {
        await next();
        return;
    }

    if (isAdminPath && (RoleId == null || RoleId == 2))
    {
        // Console.WriteLine("noadmin");
        context.Response.Redirect("/");
        return;
    }

    if (!isAdminPath && RoleId != null && RoleId != 2)
    {
        // Console.WriteLine("admin");
        context.Response.Redirect("/admin");
        return;
    }

    await next();
});

app.Use(async (context, next) =>
{
    await next();

    // if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    // {
    //     // Console.WriteLine("Not found");
    //     if (context.Request.Path.StartsWithSegments("/admin"))
    //     {
    //         // Console.WriteLine("Not found admin");
    //         context.Response.Redirect("/admin/PageNotFound");
    //     }
    //     else
    //     {
    //         // Console.WriteLine("Not found normal");
    //         context.Response.Redirect("/PageNotFound");
    //     }
    // }
});

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
