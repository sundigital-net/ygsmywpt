using System;
using System.Collections.Generic;
using System.Linq;
using YunWeiPingTai.Common;
using YunWeiPingTai.DTO;
using YunWeiPingTai.IService;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _dbContext;
        public UserService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public long AddNew(string phoneNum, string email, string name, string password)
        {
            var admins = _dbContext.AdminUsers.ToList();
            bool exsit=admins.Any(t => t.Email == email || t.PhoneNum == phoneNum);
            if(exsit)
            {
                return -1;//已存在相同的手机号或者邮箱
            }
            UserEntity entity = new UserEntity();
            entity.Name = name;
            entity.Email = email;
            entity.PhoneNum = phoneNum;
            //entity.Status = ShenHeZhuangTai.ShenQing;

            //创建6位随机字符串，作为salt
            string pwdSalt = VerifyCodeHelper.GetSingleObj().CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.MixVerifyCode, 6);
            string pwdHash = CommonHelper.CalcMD5(pwdSalt+password);
            entity.PasswordSalt = pwdSalt;
            entity.PasswordHash = pwdHash;
            _dbContext.AdminUsers.Add(entity);
            _dbContext.SaveChanges();
            return entity.Id;
        }

        public UserDTO Login(string account, string password,string ip)
        {
            var dto = new UserDTO();
            var admin = _dbContext.AdminUsers.SingleOrDefault(t => t.PhoneNum == account || t.Email == account);
            if(admin!=null)
            {
                string dbPwdHash = admin.PasswordHash;
                string adminPwdHash = CommonHelper.CalcMD5(admin.PasswordSalt + password);
                if (dbPwdHash == adminPwdHash)
                {
                    
                    admin.SigninCount++;
                    admin.LastSigninTime = DateTime.Now;
                    admin.LastSigninIP = ip;
                    _dbContext.SaveChanges();
                    dto = ToDTO(admin);
                }
            }

            return dto;
        }

        private UserDTO ToDTO(UserEntity entity)
        {
            var dto = new UserDTO();
            dto.Id = entity.Id;
            dto.CreateTime = entity.CreateTime;
            dto.Email = entity.Email;
            dto.LoginErrorTimes = entity.LoginErrorTimes;
            dto.Name = entity.Name;
            dto.PhoneNum = entity.PhoneNum;
            dto.IsLock = entity.IsLock;
            
            dto.RoleId = entity.RoleId;
            switch (entity.RoleId)
            {
                case UserRoles.SuperAdmin:
                    dto.RoleName = "超级管理员";
                    break;
                case UserRoles.Manager:
                    dto.RoleName = "部门经理";
                    break;
                case UserRoles.Engineer:
                    dto.RoleName = "工程师";
                    break;
                default:
                    dto.RoleName = "未知";
                    break;
            }
            dto.LastLoginErrorDateTime = entity.LastLoginErrorDateTime;
            dto.SigninCount = entity.SigninCount;
            dto.LastSigninTime = entity.LastSigninTime;
            dto.LastSigninTimeStr = entity.LastSigninTime == null
                ? string.Empty
                : entity.LastSigninTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            dto.LastSigninIP = entity.LastSigninIP;
            return dto;
        }

        public UserDTO GetUser(string account)
        {
            var user = _dbContext.AdminUsers.SingleOrDefault(t => t.Email == account || t.PhoneNum == account);
            return user == null?null: ToDTO(user);
        }

        public UserDTO GetUser(long id)
        {
            var user = _dbContext.AdminUsers.SingleOrDefault(t => t.Id==id);
            return user == null ? null : ToDTO(user);
        }

        public UserDTO[] GetAll()
        {
            var users = _dbContext.AdminUsers.ToList();
            List<UserDTO> list = new List<UserDTO>();
            foreach(var user in users)
            {
                list.Add(ToDTO(user));
            }
            return list.ToArray();
        }

        public void ChangePwd(string account, string password)
        {
            var user = _dbContext.AdminUsers.SingleOrDefault(t => t.Email == account || t.PhoneNum == account);
            if(user==null)
            { throw  new ArgumentException("用户信息不存在："+account);}
            string pwdSalt = VerifyCodeHelper.GetSingleObj().CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.MixVerifyCode, 6);
            string pwdHash = CommonHelper.CalcMD5(pwdSalt + password);
            user.PasswordSalt = pwdSalt;
            user.PasswordHash = pwdHash;
            _dbContext.SaveChanges();
        }

        public void MarkDeleted(long id)
        {
            var user = _dbContext.AdminUsers.SingleOrDefault(t => t.Id == id);
            if (user == null)
            { throw new ArgumentException("用户信息不存在：id=" + id); }

            user.IsDeleted = true;
            _dbContext.SaveChanges();
        }

        public void UpdateStatus(long id, bool status)
        {
            var user = _dbContext.AdminUsers.SingleOrDefault(t => t.Id == id);
            if (user == null)
            { throw new ArgumentException("用户信息不存在：id=" + id); }

            user.IsLock = status;
            _dbContext.SaveChanges();
        }
    }
}
