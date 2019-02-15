using System;
using System.Collections.Generic;
using System.Text;

namespace YunWeiPingTai.DTO
{
    public class WorkLogDTO:BaseDTO
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string LogContent { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadTime { get; set; }
    }
}
