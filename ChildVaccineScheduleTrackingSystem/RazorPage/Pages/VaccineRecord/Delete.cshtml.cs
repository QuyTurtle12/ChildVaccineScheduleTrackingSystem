using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace RazorPage.Pages.VaccineRecord
{
    public class DeleteModel : PageModel
    {
        private readonly IVaccineRecordService _vaccineRecordService;

        public DeleteModel(IVaccineRecordService vaccineRecordService)
        {
            _vaccineRecordService = vaccineRecordService;
        }


        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            
            var result = await _vaccineRecordService.DeleteAsync(id);
            if (!result)
            {
                return BadRequest("Invalid id");
            }

            return RedirectToPage("./Index");
        }
    }
}
