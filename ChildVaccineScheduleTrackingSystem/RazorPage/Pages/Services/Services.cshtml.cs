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
                    Name = "G�i vaccine c? b?n",
                    Description = "D�nh cho tr? em t? 0-2 tu?i, bao g?m c�c lo?i vaccine thi?t y?u.",
                    Price = 1500000
                },
                new Service
                {
                    Name = "G�i vaccine m? r?ng",
                    Description = "D�nh cho tr? em v� ng??i l?n, bao g?m vaccine c�m, HPV, vi�m gan B.",
                    Price = 2500000
                },
                new Service
                {
                    Name = "Ti�m vaccine c�m",
                    Description = "Ph�ng ng?a c�m m�a cho m?i l?a tu?i.",
                    Price = 300000
                },
                new Service
                {
                    Name = "Ti�m vaccine HPV",
                    Description = "Ph�ng ng?a ung th? c? t? cung cho ph? n? t? 9-26 tu?i.",
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