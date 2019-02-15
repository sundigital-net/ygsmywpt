using System;
using System.Collections.Generic;
using System.Text;

namespace YunWeiPingTai.Service.Entity
{
    public class WorkLogEntity:BaseEntity
    {
        public long UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public string LogContent { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadTime { get; set; }
        public ICollection<WorkLogReplyEntity> WorkLogReplies { get; set; }
    }
}
