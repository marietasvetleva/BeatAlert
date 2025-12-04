using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeatAlert.Web.Pages;

public class Login : PageModel
{
    private readonly LoginService _loginService;

    public Login(LoginService loginService)
    {
        _loginService = loginService;
    }
    
    
    [BindProperty]
    public LoginRequestDTO Input { get; set; } = new();
    
    public string ErrorMessage { get; set; }
    
    
    public void OnGet()
    {
        
    }

    public IActionResult OnPost()
    {
        LoginResultDTO result = _loginService.Login(Input);

        if (!result.Success)
        {
            ErrorMessage = result.ErrorMessage;

            return Page();
        }
        
        return RedirectToPage("/Index");
    }
}