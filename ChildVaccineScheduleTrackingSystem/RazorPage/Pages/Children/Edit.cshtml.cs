﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.Base;
using Data.Entities;

namespace RazorPage.Pages.Children
{
    public class EditModel : PageModel
    {
        private readonly Data.Base.ChildVaccineScheduleDbContext _context;

        public EditModel(Data.Base.ChildVaccineScheduleDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Child Child { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child =  await _context.Children.FirstOrDefaultAsync(m => m.Id == id);
            if (child == null)
            {
                return NotFound();
            }
            Child = child;
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Child).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChildExists(Child.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ChildExists(Guid id)
        {
            return _context.Children.Any(e => e.Id == id);
        }
    }
}
