using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;
using BusinessLogic.DTOs;

namespace RazorPage.Pages.VaccineRecord
{
    public class IndexModel : PageModel
    {
        private readonly IVaccineRecordService _vaccineRecordService;

        public IndexModel(IVaccineRecordService vaccineRecordService)
        {
            _vaccineRecordService = vaccineRecordService;
        }

        public IEnumerable<GetVaccineRecordDto> VaccineRecord { get;set; } = default!;

        public async Task OnGetAsync()
        {
            VaccineRecord = await _vaccineRecordService.GetAllAsync();
        }
    }
}
