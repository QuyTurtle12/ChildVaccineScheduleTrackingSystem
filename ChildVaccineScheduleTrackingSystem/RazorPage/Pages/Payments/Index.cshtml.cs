using AutoMapper;
using BusinessLogic.DTOs.PaymentDTO;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Payments
{
    public class IndexModel : PageModel
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;

        public PaginatedList<GetPaymentDTO> Payments { get; set; }
        public IndexModel(IPaymentService paymentService, IMapper mapper, IJwtTokenService jwtTokenService)
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }

        // GET: Get and Search
        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 3, string? paymentMethodSearch = null, decimal? fromAmountSearch = null, 
                                    decimal? toAmountSearch = null, string? nameSearch = null, int? statusSearch = null)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            var userRole = _jwtTokenService.GetRole(jwtToken);
            ViewData["JwtToken"] = jwtToken;
            ViewData["UserRole"] = userRole;
            Console.WriteLine($"JWT Token: {jwtToken}");
            // Fetch paginated search categories
            Payments = await _paymentService.GetPayments(pageNumber, pageSize, null, paymentMethodSearch, fromAmountSearch, toAmountSearch, nameSearch, statusSearch);
        }
    }
}
