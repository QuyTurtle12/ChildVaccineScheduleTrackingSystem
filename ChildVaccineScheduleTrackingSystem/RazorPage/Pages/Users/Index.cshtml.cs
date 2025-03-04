using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Data.Base;
using Data.Entities;

namespace RazorPage.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly Data.Base.ChildVaccineScheduleDbContext _context;

        public IndexModel(Data.Base.ChildVaccineScheduleDbContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            User = await _context.Users
                .Include(u => u.Role).ToListAsync();
        }
    }
}
