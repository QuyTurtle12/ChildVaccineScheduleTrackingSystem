using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Data.Base;
using Data.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace RazorPage.Pages.VaccineRecord
{
    public class CreateModel : PageModel
    {
        private readonly IVaccineRecordService _vaccineRecordService;

        public CreateModel(IVaccineRecordService vaccineRecordService)
        {
            _vaccineRecordService = vaccineRecordService;
        }


        public IActionResult OnGet()
        {
        
            return Page();
        }

        [BindProperty]
        public PostVaccineRecordDto VaccineRecord { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _vaccineRecordService.CreateAsync(VaccineRecord);
            if (result == null)
            {
                return BadRequest("Create vaccine record fail");
            }

            return RedirectToPage("./Index");
        }
    }
}
