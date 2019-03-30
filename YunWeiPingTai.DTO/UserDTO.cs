using System;
using System.Collections.Generic;
using System.Text;

namespace YunWeiPingTai.DTO
{
    public class UserDTO:BaseDTO
    {
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public int LoginErrorTimes { get; set; }//登陆错误次数
        public bool IsLock { get; set; } //审核状态
        public  string StatusName { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime? LastSigninTime { get; set; }
        public string LastSigninTimeStr { get; set; }
        public long SigninCount { get; set; }
        public string LastSigninIP { get; set; }
        public DateTime? LastLoginErrorDateTime { get; set; }//最后一次登录错误时间
    }
}
