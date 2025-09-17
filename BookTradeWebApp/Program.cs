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
builder.Services.AddScoped<IS_BookExchange, S_BookExchange>();
builder.Services.AddScoped<IS_File, S_File>();
builder.Services.AddScoped<IS_Category, S_Category>();
builder.Services.AddScoped<IS_Book, S_Book>();
builder.Services.AddScoped<IS_Cart, S_Cart>();

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
    name: "BookExchange",
    pattern: "book-exchange",
    defaults: new { controller = "BookExchange", action = "Index" });
app.MapControllerRoute(
    name: "Book",
    pattern: "book",
    defaults: new { controller = "Book", action = "Index" });

app.UseAuthorization();

app.Run();
