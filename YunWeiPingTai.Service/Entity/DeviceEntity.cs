#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.Service.Entity
* 项目描述 ：
* 类 名 称 ：DeviceEntity
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.Service.Entity
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-09 8:59:56
* 更新时间 ：2019-03-09 8:59:56
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ Hany 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace YunWeiPingTai.Service.Entity
{
    public class DeviceEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Version { get; set; }//版本型号
        public string Maker { get; set; }
    }
}
