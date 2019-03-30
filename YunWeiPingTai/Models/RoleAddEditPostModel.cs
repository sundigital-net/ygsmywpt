using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Models
{
    public class RoleAddEditPostModel
    {
        public  long Id { get; set; }
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "单位名称")]
        public string Name { get; set; }
        public string Remark { get; set; }
        public long[] MenuIds { get; set; }
    }
}
