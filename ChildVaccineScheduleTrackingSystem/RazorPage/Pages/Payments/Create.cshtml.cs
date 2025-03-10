using BusinessLogic.DTOs.PaymentDTO;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Payments
{
    public class CreateModel : PageModel
    {
        private readonly IPaymentService _paymentService;
        [BindProperty]
        public PostPaymentDTO Payment { get; set; }

        public CreateModel(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: Create Appointment
        public IActionResult OnGet()
        {
            return Page();
        }


        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _paymentService.CreatePayment(Payment);
            return RedirectToPage("/Index");
        }
    }
}
