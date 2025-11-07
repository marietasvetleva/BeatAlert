using BeatAlert.Logic.Services;
using BeatAlert.Logic.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using BeatAlert.Logic.Interfaces;

namespace BeatAlert.Presentation.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHomeService _homeService;

        public List<Album> Albums { get; set; }

        public IndexModel(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public void OnGet()
        {
            Albums = _homeService.GetAlbums();
        }
    }
}