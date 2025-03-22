using BusinessLogic.DTOs.PaymentDTO;
using BusinessLogic.Interfaces;
using Data.Entities;
using Data.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public List<SelectListItem> StatusList { get; set; }


        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var payment = await _paymentService.GetPaymentById(id);
            if (payment == null)
            {
                return NotFound("Payment not found!");
            }
            // Populate status dropdown list
            StatusList = Enum.GetValues(typeof(EnumPayment))
                .Cast<EnumAppointment>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }).ToList();


            UpdatedPayment = new PutPaymentDTO
            {
                Id = payment.Id,
                AppointmentId = payment.AppointmentId, // Ensure this is correctly mapped
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Name = payment.Name,
                Status = payment.Status,
                CreatedBy = payment.CreatedBy,
                CreatedTime = payment.CreatedTime,
                LastUpdatedBy = payment.LastUpdatedBy,
                LastUpdatedTime = payment.LastUpdatedTime,
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
