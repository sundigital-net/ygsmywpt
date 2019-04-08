using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Models
{
    public class DeviceAddEditPostModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public  string Version { get; set; }
        [Required]
        public string Maker { get; set; }
    }
}
