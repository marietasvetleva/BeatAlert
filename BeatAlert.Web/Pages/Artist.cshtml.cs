using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeatAlert.Web.Pages;

[Authorize]
public class Artist : PageModel
{
    private readonly ArtistService _artistService;

    public Artist(ArtistService artistService)
    {
        _artistService = artistService;
    }
    
    public ArtistDetailsDTO ArtistDetails { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsFollowed = false;
    
    public void OnGet(int id)
    {
        try
        {
            ArtistDetails = _artistService.GetById(id);
        }

        catch (ServiceException exception)
        {
            ErrorMessage = exception.Message;
        }
        
    }
}