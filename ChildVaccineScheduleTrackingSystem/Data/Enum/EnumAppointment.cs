using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Enum
{
    public enum EnumAppointment
    {
        [Display(Name = "Chưa giải quyết")]
        Pending = 0,

        [Display(Name = "Đã xác nhận")]
        Confirmed = 1,

        [Display(Name = "Đã hủy")]
        Canceled = 2,

        [Display(Name = "Hoàn thành")]
        Completed = 3
    }


}
