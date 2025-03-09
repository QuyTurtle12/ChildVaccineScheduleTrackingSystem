using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Data.Base;
using Data.Entities;

namespace RazorPage.Pages.Children
{
    public class CreateModel : PageModel
    {
        private readonly Data.Base.ChildVaccineScheduleDbContext _context;

        public CreateModel(Data.Base.ChildVaccineScheduleDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Child Child { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Children.Add(Child);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
