using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace BeatAlert.Web.Pages
{
    [Authorize]
    public class HomeModel : PageModel
    {
        private readonly IHomeService _homeService;

        public List<AlbumDTO> Albums { get; set; }
        public string FirstName { get; set; }

        public HomeModel(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public void OnGet()
        {
            Albums = _homeService.GetAlbums();
            
            FirstName = User.FindFirst("FirstName")?.Value 
                        ?? User.Identity?.Name;
        }
    }
}