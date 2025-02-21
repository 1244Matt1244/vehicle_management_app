using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project.MVC.Pages
{
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public void OnGet()
        {
            // Logic for the Error page
        }
    }
}
