using BusinessLogic.DTOs.PaymentDTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Payments
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IPaymentService _paymentService;

        public DeleteModel(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [BindProperty]
        public GetPaymentDTO Payment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var payment = await _paymentService.GetPaymentById(id);

            if (payment == null)
            {
                return NotFound();
            }
            else
            {
                Payment = payment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _paymentService.DeletePayment(id);
            return RedirectToPage("./Index");
        }
    }
}
