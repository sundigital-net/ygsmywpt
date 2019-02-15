using System;
using System.Collections.Generic;
using System.Text;
using YunWeiPingTai.DTO;

namespace YunWeiPingTai.IService
{
    public interface IUnitService:IServiceSupport
    {
        long AddNew(string name, string address, string linkMan, string tel, string phoneNum);
        UnitDTO[] GetAll();
        UnitDTO GetById(long id);
    }
}
