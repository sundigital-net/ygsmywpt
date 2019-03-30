using System;
using System.Collections.Generic;

namespace YunWeiPingTai.Service.Entity
{
    public class UserEntity:BaseEntity
    {
        public string Name { get; set; }//姓名 50
        public string PhoneNum { get; set; }//20
        public string Email { get; set; }//100
        public string PasswordSalt { get; set; }//100
        public string PasswordHash { get; set; }//100
        public int LoginErrorTimes { get; set; }//登陆错误次数
        public bool IsLock { get; set; } //状态
        public long RoleId { get; set; }//用户角色
        public DateTime? LastSigninTime { get; set; }
        public long SigninCount { get; set; }
        public string LastSigninIP { get; set; }
        public DateTime? LastLoginErrorDateTime { get; set; }//最后一次登录错误时间
        public ICollection<WorkLogEntity> WorkLogs { get; set; }
        public ICollection<WorkLogReplyEntity> WorkLogReplies { get; set; }
    }
}
