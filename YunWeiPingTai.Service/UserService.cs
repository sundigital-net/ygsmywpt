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
            entity.Status = ShenHeZhuangTai.ShenQing;
            //创建6位随机字符串，作为salt
            string pwdSalt = VerifyCodeHelper.GetSingleObj().CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.MixVerifyCode, 6);
            string pwdHash = CommonHelper.CalcMD5(pwdSalt+password);
            entity.PasswordSalt = pwdSalt;
            entity.PasswordHash = pwdHash;
            _dbContext.AdminUsers.Add(entity);
            _dbContext.SaveChanges();
            return entity.Id;
        }

        public bool CheckLogin(string account, string password)
        {
            var admin = _dbContext.AdminUsers.SingleOrDefault(t => t.PhoneNum == account || t.Email == account);
            if(admin==null)
            {
                return false;
            }
            string dbPwdHash = admin.PasswordHash;
            string adminPwdHash = CommonHelper.CalcMD5(admin.PasswordSalt + password);
            return dbPwdHash == adminPwdHash;
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
            dto.Status = entity.Status;
            switch (entity.Status)
            {
                case ShenHeZhuangTai.ShenQing:
                    dto.StatusName = "待审核";
                    break;
                case ShenHeZhuangTai.TongGuo:
                    dto.StatusName = "正常";
                    break;
                case ShenHeZhuangTai.BoHui:
                    dto.StatusName = "未通过";
                    break;
                default:
                    dto.StatusName = "未知";
                    break;
            }
            dto.Role = entity.Role;
            switch (entity.Role)
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

        public void UpdateStatus(long id, int status)
        {
            var user = _dbContext.AdminUsers.SingleOrDefault(t => t.Id == id);
            if (user == null)
            { throw new ArgumentException("用户信息不存在：id=" + id); }

            user.Status = status;
            _dbContext.SaveChanges();
        }
    }
}
