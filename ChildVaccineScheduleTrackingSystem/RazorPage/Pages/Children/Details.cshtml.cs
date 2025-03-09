using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Data.Base;
using Data.Entities;

namespace RazorPage.Pages.Children
{
    public class DetailsModel : PageModel
    {
        private readonly Data.Base.ChildVaccineScheduleDbContext _context;

        public DetailsModel(Data.Base.ChildVaccineScheduleDbContext context)
        {
            _context = context;
        }

        public Child Child { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = await _context.Children.FirstOrDefaultAsync(m => m.Id == id);
            if (child == null)
            {
                return NotFound();
            }
            else
            {
                Child = child;
            }
            return Page();
        }
    }
}
