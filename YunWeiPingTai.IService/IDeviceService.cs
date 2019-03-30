#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.IService
* 项目描述 ：
* 类 名 称 ：IDeviceService
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.IService
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-09 9:10:43
* 更新时间 ：2019-03-09 9:10:43
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ Hany 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using YunWeiPingTai.DTO;

namespace YunWeiPingTai.IService
{
    public interface IDeviceService:IServiceSupport
    {
        DeviceDTO GetById(long id);
        DeviceDTO[] GetAll();
        long AddNew(string name, string version,string maker);
        void Delete(long id);
    }
}
