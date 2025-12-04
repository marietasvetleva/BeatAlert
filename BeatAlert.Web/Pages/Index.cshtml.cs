using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeatAlert.Web.Pages;

[AllowAnonymous]
public class Index : PageModel
{
    public void OnGet()
    {
        
    }
}