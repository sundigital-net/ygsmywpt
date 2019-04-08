using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Models
{
    public class UnitDeviceAddEditPostModel
    {
        public long Id { get; set; }
        public long UnitId { get; set; }
        public long DeviceId { get; set; }
        [Required]
        public string SNCode { get; set; }
        
    }
}
