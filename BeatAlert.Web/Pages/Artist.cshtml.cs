using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeatAlert.Web.Pages;

public class Artist : PageModel
{
    private readonly ArtistService _artistService;

    public Artist(ArtistService artistService)
    {
        _artistService = artistService;
    }
    
    public ArtistDetailsDTO ArtistDetails { get; set; }
    
    
    public void OnGet(int id)
    {
        ArtistDetails = _artistService.GetById(id);
        
    }
}