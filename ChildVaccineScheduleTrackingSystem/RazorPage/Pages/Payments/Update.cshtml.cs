using BusinessLogic.DTOs.PaymentDTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Payments
{
    public class UpdateModel : PageModel
    {
        private readonly IPaymentService _paymentService;

        public UpdateModel(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [BindProperty]
        public GetPaymentDTO Payment { get; set; } = default!;

        [BindProperty]
        public PutPaymentDTO UpdatedPayment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var payment = await _paymentService.GetPaymentById(id);
            if (payment == null)
            {
                return NotFound("Payment not found!");
            }

            UpdatedPayment = new PutPaymentDTO
            {
                Id = payment.Id,
                AppointmentId = payment.AppointmentId, // Ensure this is correctly mapped
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Name = payment.Name,
                Status = payment.Status
            };

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (UpdatedPayment == null)
            {
                return NotFound("Updated payment not found!");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _paymentService.UpdatePayment(UpdatedPayment);
            return RedirectToPage("./Index");
        }
    }
}
