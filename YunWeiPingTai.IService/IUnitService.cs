using System;
using System.Collections.Generic;
using System.Text;
using YunWeiPingTai.DTO;
using YunWeiPingTai.DTO.RequestModel;

namespace YunWeiPingTai.IService
{
    public interface IUnitService:IServiceSupport
    {
        long AddOrEdit(long id,string name, string address, string linkMan, string tel, string phoneNum);
        List<UnitDTO> GetAll();
        UnitDTO GetById(long id);
        void UnitDel(long[] unitIds);
        UnitDeviceDTO GetUnitDevice(long id);
        List<UnitDeviceDTO> GetByUnitId(long unitId);
        long AddOrEdit(long id,long unitId, long deviceId, long userId,string snCode);
        void UnitDeviceDel(long unitDeviceId);
        bool IsExistsName(string name, long id);
        TableDataModel LoadData(UnitRequestModel model);
        TableDataModel LoadData(UnitDeviceRequestModel model);
    }
}
