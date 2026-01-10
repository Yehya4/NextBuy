using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NextBuy.Models;

namespace NextBuy.Areas.Admin.Pages.Clients;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public IList<ApplicationUser> Users { get; set; } = default!;

    public async Task OnGetAsync()
    {
        // Get all users who are NOT admins (or just all users)
        // Ideally filter by role, but for now list all
        Users = await _userManager.Users.ToListAsync();
    }
}
