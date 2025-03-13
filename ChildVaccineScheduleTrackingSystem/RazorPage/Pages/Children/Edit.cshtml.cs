using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.Base;
using Data.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs.ChildrenDTO;
using AutoMapper;
using BusinessLogic.Services;

namespace RazorPage.Pages.Children
{
    public class EditModel : PageModel
    {
        private readonly IChildrenService _childrenService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public EditModel(IChildrenService childrenService, IJwtTokenService jwtTokenService, IMapper mapper)
        {
            _childrenService = childrenService;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        [BindProperty]
        public PutChildrenDTO EditChild { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GetChildrenDTO ChildDTO = await _childrenService.GetChildrenAccount(id);
            if (ChildDTO == null)
            {
                return NotFound();
            }
            EditChild = _mapper.Map<PutChildrenDTO>(ChildDTO);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(PutChildrenDTO editedChild)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _childrenService.UpdateChildrenAccount(editedChild);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message });
            }

            return RedirectToPage("./Index");
        }
    }
}
