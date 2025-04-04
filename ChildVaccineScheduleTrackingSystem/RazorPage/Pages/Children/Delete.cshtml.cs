﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Data.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.ChildrenDTO;
using BusinessLogic.Services;

namespace RazorPage.Pages.Children
{
    public class DeleteModel : PageModel
    {
        private readonly IChildrenService _childrenService;
        private readonly IJwtTokenService _jwtTokenService;

        private const string CUSTOMER_ROLE = "Customer";

        public DeleteModel(IChildrenService childrenService, IJwtTokenService jwtTokenService)
        {
            _childrenService = childrenService;
            _jwtTokenService = jwtTokenService;
        }

        [BindProperty]
        public GetChildrenDTO ChildDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole != CUSTOMER_ROLE)
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            GetChildrenDTO child = await _childrenService.GetChildrenAccount(id);
            if (child == null)
            {
                return NotFound();
            }
            else
            {
                ChildDTO = child;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            var jwtToken = HttpContext.Session.GetString("jwt_token");
            string loggedInUserRole = _jwtTokenService.GetRole(jwtToken!);

            if (loggedInUserRole == null) return Unauthorized();

            if (loggedInUserRole != CUSTOMER_ROLE)
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await _childrenService.DeleteChildrenAccountById(id);
                TempData["SuccessMessage"] = "Child deleted successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error" + ex.Message;
                throw;
            }
        }
    }
}
