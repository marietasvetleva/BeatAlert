using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace BeatAlert.Web.Pages
{
    [Authorize]
    public class HomeModel : PageModel
    {
        private readonly AlbumService _albumService;

        public HomeModel(AlbumService albumService)
        {
            _albumService = albumService;
        }

        public List<AlbumDTO> Albums { get; set; }
        public string FirstName { get; set; }

        public void OnGet()
        {
            Albums = _albumService.GetNewAlbums();
            
            FirstName = User.FindFirst("FirstName")?.Value 
                        ?? User.Identity?.Name;
        }
    }
}