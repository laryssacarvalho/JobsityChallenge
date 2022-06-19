using JobsityChallenge.Chat;
using JobsityChallenge.Chat.Data;
using JobsityChallenge.Chat.Entities;
using JobsityChallenge.Chat.Hubs;
using JobsityChallenge.Chat.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<UserEntity>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

    // User settings.
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddScoped<IRepository<MessageEntity, int>, Repository<MessageEntity, int>>();
builder.Services.AddScoped<IRepository<UserEntity, string>, Repository<UserEntity, string>>();

builder.Services.AddScoped<DbContext, ApplicationDbContext>();
builder.Services.AddHttpClient<ChatHub>();
//builder.Services.AddHostedService<StockQueueConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Chatroom/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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