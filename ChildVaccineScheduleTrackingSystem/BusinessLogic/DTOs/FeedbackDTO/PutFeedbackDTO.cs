using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.FeedbackDTO
{
    public class PutFeedbackDTO
    {
        public string Id { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
