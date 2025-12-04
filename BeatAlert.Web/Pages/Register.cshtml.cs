using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeatAlert.Web.Pages;

[AllowAnonymous]
public class Register : PageModel
{

    private readonly IRegistrationService _registrationService;

    public Register(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }
    [BindProperty]
    public MusicLoverDTO Input { get; set; } = new();
    public string ErrorMessage { get; set; }
    
    
    public void OnGet()
    {
        
    }

    public IActionResult OnPost()
    {
        RegistrationResultDTO result = _registrationService.Register(Input);

        if (!result.Success)
        {
            ErrorMessage = result.ErrorMessage;
            return Page();
        }

        return RedirectToPage("/Index");
    }
    
}