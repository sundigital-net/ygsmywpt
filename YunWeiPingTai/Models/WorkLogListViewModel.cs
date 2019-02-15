using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YunWeiPingTai.DTO;

namespace YunWeiPingTai.Models
{
    public class WorkLogListViewModel
    {
        public  WorkLogDTO[] WorkLogs { get; set; }
        public  WorkLogReplyDTO[] WorkLogReplies { get; set; }
    }
}
