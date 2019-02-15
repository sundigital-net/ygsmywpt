using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Models
{
    public class UnitAddPostModel
    {
        [Display(Name = "单位名称")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string Name { get; set; }
        [Display(Name = "单位地址")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string Address { get; set; }
        [Display(Name = "联系人")]
        [Required(ErrorMessage = "{0}是必填项")]
        public  string LinkMan { get; set; }
        public string Tel { get; set; }
        [Display(Name = "手机号码")]
        [Required(ErrorMessage = "{0}是必填项")]
        
        public string PhoneNum { get; set; }
    }
}
