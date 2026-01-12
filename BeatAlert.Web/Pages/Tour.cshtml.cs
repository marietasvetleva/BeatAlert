using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeatAlert.Web.Pages;

[Authorize]
public class Tour : PageModel
{
    private readonly TourService _tourService;

    public Tour(TourService tourService)
    {
        _tourService = tourService;
    }
    
    public TourDetailsDto TourDetails { get; set; }
    public List<ShowDTO> Show { get; set; }
    
    public void OnGet(int id)
    {
        TourDetails = _tourService.GetById(id);
    }
}