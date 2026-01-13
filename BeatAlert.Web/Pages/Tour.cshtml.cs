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
    public string ErrorMessage { get; set; }
    
    public void OnGet(int id)
    {
        try
        {
            TourDetails = _tourService.GetById(id);
        }

        catch (ServiceException exception)
        {
            ErrorMessage = exception.Message;
        }
        
    }
}