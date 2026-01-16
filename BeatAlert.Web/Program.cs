using BeatAlert.DAL.Repositories;
using BeatAlert.Logic.Interfaces___Repositories;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Services;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";          // where to send users when not logged in
        options.LogoutPath = "/Logout";       
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // how long "remember me" lasts
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();



builder.Services.AddScoped<IAlbumRepository>(_ => new AlbumRepository(connectionString));
builder.Services.AddScoped<IArtistRepository>(_ => new ArtistRepository(connectionString));
builder.Services.AddScoped<ISongRepository>(_ => new SongRepository(connectionString));
builder.Services.AddScoped<IShowRepository>(_ => new ShowRepository(connectionString));
builder.Services.AddScoped<ITourRepository>(_ => new TourRepository(connectionString));
builder.Services.AddScoped<IUserRepository>(_ => new UserRepository(connectionString));
builder.Services.AddScoped<IFollowRepository>(_ => new FollowRepository(connectionString));


builder.Services.AddScoped<IGlobalSearchService, GlobalSearchService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<ArtistService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<TourService>();
builder.Services.AddScoped<AlbumService>();
builder.Services.AddScoped<FollowService>();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();