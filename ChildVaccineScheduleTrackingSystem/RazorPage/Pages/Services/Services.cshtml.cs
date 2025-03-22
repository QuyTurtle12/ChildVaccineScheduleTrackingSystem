using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace RazorPage.Pages
{
    public class ServicesModel : PageModel
    {
        public List<Service> Services { get; set; }

        public void OnGet()
        {
            
            Services = new List<Service>
            {
                new Service
                {
                    Name = "Gói vaccine c? b?n",
                    Description = "Dành cho tr? em t? 0-2 tu?i, bao g?m các lo?i vaccine thi?t y?u.",
                    Price = 1500000
                },
                new Service
                {
                    Name = "Gói vaccine m? r?ng",
                    Description = "Dành cho tr? em và ng??i l?n, bao g?m vaccine cúm, HPV, viêm gan B.",
                    Price = 2500000
                },
                new Service
                {
                    Name = "Tiêm vaccine cúm",
                    Description = "Phòng ng?a cúm mùa cho m?i l?a tu?i.",
                    Price = 300000
                },
                new Service
                {
                    Name = "Tiêm vaccine HPV",
                    Description = "Phòng ng?a ung th? c? t? cung cho ph? n? t? 9-26 tu?i.",
                    Price = 1200000
                }
            };
        }
    }

    public class Service
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}