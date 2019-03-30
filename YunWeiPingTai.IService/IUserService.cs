using System;
using System.Collections.Generic;
using System.Text;
using YunWeiPingTai.DTO;

namespace YunWeiPingTai.IService
{
    public interface IUserService:IServiceSupport
    {
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="account">邮箱或者手机号</param>
        /// <returns></returns>
        UserDTO GetUser(string account);
        UserDTO GetUser(long id);
        UserDTO[] GetAll();
        UserDTO Login(string account, string password,string ip);//手机号或者邮箱号
        long AddNew(string phoneNum, string email, string name, string password);
        void ChangePwd(string account, string password);
        void MarkDeleted(long id);
        void UpdateStatus(long id, bool status);
    }
}
