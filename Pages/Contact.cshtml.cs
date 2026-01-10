using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NextBuy.Pages;

public class ContactModel : PageModel
{
    public void OnGet()
    {
    }

    public void OnPost()
    {
        // Logic to send email would go here
        // For now, just reload
    }
}
