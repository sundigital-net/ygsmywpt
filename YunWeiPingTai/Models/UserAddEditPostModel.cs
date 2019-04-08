using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Models
{
    public class UserAddEditPostModel
    {
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public long RoleId { get; set; }
        [Required]
        public string PhoneNum { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsLock { get; set; }
    }
}
