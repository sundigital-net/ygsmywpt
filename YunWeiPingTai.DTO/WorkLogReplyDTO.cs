using System;
using System.Collections.Generic;
using System.Text;

namespace YunWeiPingTai.DTO
{
    public class WorkLogReplyDTO:BaseDTO
    {
        public long WorkLogId { get; set; }
        public string Reply { get; set; }
        public long AdminId { get; set; }
        public string AdminName { get; set; }
    }
}
