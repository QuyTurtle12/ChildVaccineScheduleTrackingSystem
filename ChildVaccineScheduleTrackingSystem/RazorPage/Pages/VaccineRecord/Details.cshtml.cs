using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace RazorPage.Pages.VaccineRecord
{
    public class DetailsModel : PageModel
    {
        private readonly IVaccineRecordService _vaccineRecordService;

        public DetailsModel(IVaccineRecordService vaccineRecordService)
        {
            _vaccineRecordService = vaccineRecordService;
        }

        public GetVaccineRecordDto VaccineRecord { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var vaccinerecord = await _vaccineRecordService.GetByIdAsync(id);
            if (vaccinerecord == null)
            {
                return NotFound();
            }
            else
            {
                VaccineRecord = vaccinerecord;
            }
            return Page();
        }
    }
}
