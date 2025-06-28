using BookTradeWebApp.Extensions;
using BookTradeWebApp.Services;

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
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");
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
