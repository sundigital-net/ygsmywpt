using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YunWeiPingTai.Service.Entity;
using YunWeiPingTai.Common;

namespace YunWeiPingTai.Service
{
    public static class MyDbContextInsertSeed
    {
        public static void EnsureSeedDataForContext(this MyDbContext dbContext)
        {
            if (dbContext.AdminUsers.Count() > 0)
            {
                return;
            }
            var admin = new UserEntity();
            admin.Email = "364572026@qq.com";
            admin.Name = "张汉英";
            admin.PhoneNum = "15376261308";
            admin.IsLock = false;
            string pwdSalt = VerifyCodeHelper.GetSingleObj().CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.MixVerifyCode, 6);
            string pwdHash = CommonHelper.CalcMD5(pwdSalt + "123456");
            admin.PasswordSalt = pwdSalt;
            admin.PasswordHash = pwdHash;
            dbContext.AdminUsers.Add(admin);
            dbContext.SaveChanges();
        }
    }
}
