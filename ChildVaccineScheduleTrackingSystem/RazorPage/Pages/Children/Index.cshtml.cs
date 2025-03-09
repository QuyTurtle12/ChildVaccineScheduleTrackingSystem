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
    public class IndexModel : PageModel
    {
        private readonly Data.Base.ChildVaccineScheduleDbContext _context;

        public IndexModel(Data.Base.ChildVaccineScheduleDbContext context)
        {
            _context = context;
        }

        public IList<Child> Child { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Child = await _context.Children
                .Include(c => c.User).ToListAsync();
        }
    }
}
