using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeatAlert.Web.Pages;

public class Search : PageModel
{
    private readonly IGlobalSearchService _globalSearchService;

    public Search(IGlobalSearchService globalSearchService)
    {
        _globalSearchService = globalSearchService;
    }
    
    [BindProperty(SupportsGet = true)]
    public string? Query { get; set; }

    public GlobalSearchResultsDto Result { get; private set; } = new();
    
    
    public void OnGet()
    {
        Result = _globalSearchService.Search(Query);
    }
}