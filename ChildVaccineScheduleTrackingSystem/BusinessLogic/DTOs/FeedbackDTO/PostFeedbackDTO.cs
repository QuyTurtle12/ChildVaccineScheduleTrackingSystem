using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.FeedbackDTO
{
    public class PostFeedbackDTO
    {
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public string UserId { get; set; } 
    }
}
