using System;
using System.Collections.Generic;
using System.Text;

namespace YunWeiPingTai.Service.Entity
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } 
    }
}
