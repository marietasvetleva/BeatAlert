using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BeatAlert.Web.Pages;

[AllowAnonymous] 
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
    public bool RememberMe { get; set; }
    
    
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = _loginService.Login(Input);

        if (result == null || !result.Success)
        {
            ErrorMessage = result?.ErrorMessage ?? "Login failed.";
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
            new Claim(ClaimTypes.Name, Input.Identifier ?? string.Empty),
            new Claim("FirstName", result.FirstName ?? string.Empty)
        };

        if (!string.IsNullOrWhiteSpace(Input.Identifier) && Input.Identifier.Contains("@"))
        {
            claims.Add(new Claim(ClaimTypes.Email, Input.Identifier));
        }

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = RememberMe
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties);

        return RedirectToPage("/Home");
    }
}