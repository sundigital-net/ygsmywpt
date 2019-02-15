using System;
using System.Collections.Generic;
using System.Text;

namespace YunWeiPingTai.Service.Entity
{
    public class WorkLogReplyEntity:BaseEntity
    {
        public long WorkLogId { get; set; }
        //public virtual WorkLogEntity WorkLog { get; set; }
        public string Reply { get; set; }
        public long UserId { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
