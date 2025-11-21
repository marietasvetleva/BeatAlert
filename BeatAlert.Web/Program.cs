using BeatAlert.DAL.Repositories;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Services;

var builder = WebApplication.CreateBuilder(args);

// Services (ADD ALL BEFORE Build)
builder.Services.AddRazorPages();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection");

builder.Services.AddScoped<IAlbumRepository>(_ => new AlbumRepository(connectionString));
builder.Services.AddScoped<IArtistRepository>(_ => new ArtistRepository(connectionString));
builder.Services.AddScoped<ISongRepository>(_ => new SongRepository(connectionString));
builder.Services.AddScoped<IShowRepository>(_ => new ShowRepository(connectionString));
builder.Services.AddScoped<ITourRepository>(_ => new TourRepository(connectionString));


builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IGlobalSearchService, GlobalSearchService>();
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
app.UseAuthorization();

app.MapRazorPages();
app.Run();