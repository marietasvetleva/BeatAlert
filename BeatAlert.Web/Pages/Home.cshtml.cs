using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace BeatAlert.Presentation.Pages
{
    public class HomeModel : PageModel
    {
        private readonly IHomeService _homeService;

        public List<AlbumDTO> Albums { get; set; }

        public HomeModel(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public void OnGet()
        {
            Albums = _homeService.GetAlbums();
        }
    }
}