using JobsityChallenge.Chat.BackgroundServices;
using JobsityChallenge.Chat.Data;
using JobsityChallenge.Chat.Entities;
using JobsityChallenge.Chat.Hubs;
using JobsityChallenge.Chat.Repositories;
using JobsityChallenge.Chat.Services;
using JobsityChallenge.Chat.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));


//Identity
builder.Services.AddDefaultIdentity<UserEntity>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

    // User settings.
    options.User.RequireUniqueEmail = true;
});

//Repositories
builder.Services.AddScoped<IRepository<MessageEntity, int>, Repository<MessageEntity, int>>();
builder.Services.AddScoped<IRepository<UserEntity, string>, Repository<UserEntity, string>>();

//Services
builder.Services.AddScoped<IBotApiService, BotApiService>();
builder.Services.AddHttpClient<BotApiService>();

//Hosted Services
builder.Services.AddHostedService<StockQueueConsumerService>();

builder.Services.AddScoped<DbContext, ApplicationDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Chatroom/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chatroom}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapHub<ChatHub>("/chat");

app.Run();