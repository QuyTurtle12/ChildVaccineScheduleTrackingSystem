//using BusinessLogic.Interfaces;
//using BusinessLogic.DTOs.SystemAccountDTOs;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//namespace RazorPage.Pages
//{
//    public class LoginModel : PageModel
//    {
//        private readonly ISystemAccountService _systemAccountService;
//        private readonly ILogger<LoginModel> _logger;

//        [BindProperty]
//        public LoginDTO LoginDTO { get; set; }

//        public string ErrorMessage { get; set; }

//        public LoginModel(ISystemAccountService systemAccountService, ILogger<LoginModel> logger)
//        {
//            _systemAccountService = systemAccountService;
//            _logger = logger;
//        }

//        public void OnGet()
//        {
//            var jwtToken = HttpContext.Session.GetString("jwt_token");
//            ViewData["JwtToken"] = jwtToken;

//            if (!string.IsNullOrEmpty(jwtToken))
//            {
//                _logger.LogInformation("JWT token found in session; redirecting to home page.");
//                Response.Redirect("/Index");
//            }
//        }

//        public async Task<IActionResult> OnPostAsync()
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Login attempt with invalid model state.");
//                return Page();
//            }

//            try
//            {
//                var token = await _systemAccountService.Login(LoginDTO);

//                if (!string.IsNullOrEmpty(token))
//                {
//                    HttpContext.Session.SetString("jwt_token", token);
//                    _logger.LogInformation("JWT token stored in session.");
//                    return RedirectToPage("/Index");
//                }
//                else
//                {
//                    ErrorMessage = "Invalid credentials, please try again.";
//                    _logger.LogWarning("Login failed: JWT token is null.");
//                    return Page();
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Login error for email: {Email}", LoginDTO.AccountEmail);
//                ErrorMessage = "Invalid credentials, please try again.";
//                return Page();
//            }
//        }
//    }
//}

