using System;
using System.Collections.Generic;
using System.Text;
using YunWeiPingTai.DTO;

namespace YunWeiPingTai.IService
{
    public interface IUnitService:IServiceSupport
    {
        long AddOrEdit(long id,string name, string address, string linkMan, string tel, string phoneNum);
        List<UnitDTO> GetAll();
        UnitDTO GetById(long id);
        void UnitDel(long unitId);
        UnitDeviceDTO GetUnitDevice(long id);
        List<UnitDeviceDTO> GetByUnitId(long unitId);
        long AddOrEdit(long unitId, long deviceId, long userId,string snCode);
        void UnitDeviceDel(long unitDeviceId);
    }
}
