using BusinessLogic.DTOs.PaymentDTO;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Data.Entities;
using Data.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RazorPage.Pages.Payments
{
    [Authorize(Roles = "Staff")]
    public class CreateModel : PageModel
    {
        private readonly IPaymentService _paymentService;
        [BindProperty]
        public PostPaymentDTO Payment { get; set; }

        public List<SelectListItem> StatusList { get; set; }

        public CreateModel(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: Create Payment
        public IActionResult OnGet(Guid? appointmentId)
        {
            Payment = new PostPaymentDTO();

            if (appointmentId.HasValue)
            {
                Payment.AppointmentId = appointmentId.Value;
            }
            StatusList = Enum.GetValues(typeof(EnumPayment))
                .Cast<EnumPayment>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    // Text = e.ToString()
                    Text = e.GetType()
                .GetField(e.ToString())
                ?.GetCustomAttribute<DisplayAttribute>()?
                .Name ?? e.ToString()
                }).ToList();

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
            return RedirectToPage("./Index");

        }
    }
}
