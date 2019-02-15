using System;
using System.Collections.Generic;
using System.Text;

namespace YunWeiPingTai.Common
{
    public class AjaxResult
    {
        public string Status { get; set; }
        public string ErrorMsg { get; set; }
        public object Data { get; set; }
    }
}
