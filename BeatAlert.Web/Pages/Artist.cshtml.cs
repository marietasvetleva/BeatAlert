using System.Security.Claims;
using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeatAlert.Web.Pages;

[Authorize]
public class Artist : PageModel
{
    private readonly ArtistService _artistService;
    private readonly FollowService _followService;

    public Artist(ArtistService artistService, FollowService followService)
    {
        _artistService = artistService;
        _followService = followService;
    }

    public ArtistDetailsDTO ArtistDetails { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsFollowed { get; set; }

    public void OnGet(int id)
    {
        try
        {
            ArtistDetails = _artistService.GetById(id);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var music_lover_id))
            {
                IsFollowed = false;
                return;
            }

            IsFollowed = _followService.IsFollowed(music_lover_id, id);
        }

        catch (ServiceException exception)
        {
            ErrorMessage = exception.Message;
        }

    }

    public IActionResult OnPost(int id)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var musicLoverId))
        {
            return RedirectToPage(new { id });
        }

        _followService.ToggleFollow(musicLoverId, id);

        return RedirectToPage(new { id });
    }
}