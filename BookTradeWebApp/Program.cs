using BookTradeWebApp.Extensions;
using BookTradeWebApp.Services;
using VNPAY.NET;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("ApiClient", httpClient =>
{
    string apiUrl = builder.Configuration.GetValue<string>("ApiConfig:ApiUrl")!;
    httpClient.BaseAddress = new Uri(apiUrl);
}).AddHttpMessageHandler<JwtAuthorizationHandler>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<JwtAuthorizationHandler>();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<IS_Auth, S_Auth>();
builder.Services.AddScoped<IS_File, S_File>();
builder.Services.AddScoped<IS_Category, S_Category>();
builder.Services.AddScoped<IS_Book, S_Book>();
builder.Services.AddScoped<IS_Cart, S_Cart>();
builder.Services.AddScoped<IS_Order, S_Order>();
builder.Services.AddScoped<IS_Payment, S_Payment>();
builder.Services.AddScoped<IS_Notification, S_Notification>();

builder.Services.AddSingleton<IVnpay, Vnpay>();

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

app.UseSession();
app.UseRouting();

app.MapControllerRoute(
    name: "Login",
    pattern: "login",
    defaults: new { controller = "Auth", action = "Login" });

app.MapControllerRoute(
    name: "RegisterSeller",
    pattern: "register-seller",
    defaults: new { controller = "Auth", action = "RegisterSeller" });

#region Admin
app.MapAreaControllerRoute(
    name: "Dashboard",
    areaName: "Admin",
    pattern: "admin/dashboard",
    defaults: new { controller = "Dashboard", action = "Index" });

app.MapAreaControllerRoute(
    name: "ManageShop",
    areaName: "Admin",
    pattern: "admin/manage-shop",
    defaults: new { controller = "ManageShop", action = "Index" });

app.MapAreaControllerRoute(
    name: "AdminDefault",
    areaName: "Admin",
    pattern: "{area}/{controller}/{action}/{id?}");
#endregion

#region Seller
app.MapAreaControllerRoute(
    name: "Dashboard",
    areaName: "Seller",
    pattern: "seller/dashboard",
    defaults: new { controller = "Dashboard", action = "Index" });

app.MapAreaControllerRoute(
    name: "ManageBook",
    areaName: "Seller",
    pattern: "seller/manage-book",
    defaults: new { controller = "ManageBook", action = "Index" });

app.MapAreaControllerRoute(
    name: "ManageOrder",
    areaName: "Seller",
    pattern: "seller/manage-order",
    defaults: new { controller = "ManageOrder", action = "Index" });

app.MapAreaControllerRoute(
    name: "SellerDefault",
    areaName: "Seller",
    pattern: "{area}/{controller}/{action}/{id?}");
#endregion

#region Member
app.MapAreaControllerRoute(
    name: "Shop",
    areaName: "Member",
    pattern: "member/shop",
    defaults: new { controller = "Shop", action = "Index" });

app.MapAreaControllerRoute(
    name: "Cart",
    areaName: "Member",
    pattern: "member/cart",
    defaults: new { controller = "Cart", action = "Index" });

app.MapAreaControllerRoute(
    name: "MemberDefault",
    areaName: "Member",
    pattern: "{area}/{controller}/{action}/{id?}");
#endregion

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseAuthorization();

app.Run();
