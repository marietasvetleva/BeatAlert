using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeatAlert.Web.Pages;

[Authorize]
public class Album : PageModel
{
    private readonly AlbumService _albumService;

    public Album(AlbumService albumService)
    {
        _albumService = albumService;
    }
    
    public AlbumDTO AlbumDetails { get; set; }
    public string ErrorMessage { get; set; }
    
    
    public void OnGet(int id)
    {
        try
        {
            AlbumDetails = _albumService.GetById(id);
        }

        catch (ServiceException exception)
        {
            ErrorMessage = exception.Message;
        }
    }
}