using BeatAlert.Logic.DTOs;
using BeatAlert.Logic;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeatAlert.Web.Pages;

[Authorize]
public class Search : PageModel
{
    private readonly IGlobalSearchService _globalSearchService;

    public Search(IGlobalSearchService globalSearchService)
    {
        _globalSearchService = globalSearchService;
    }
    
    [BindProperty(SupportsGet = true)]
    public string? Query { get; set; }

    public string ErrorMessage { get; set; }
    public GlobalSearchResultsDto Result { get; private set; } = new();
    
    
    public void OnGet()
    {
        try
        {
            Result = _globalSearchService.Search(Query);
        }

        catch (ServiceException exception)
        {
            ErrorMessage = exception.Message;
        }
       
    }
}