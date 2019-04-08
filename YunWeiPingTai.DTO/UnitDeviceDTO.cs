﻿#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.DTO
* 项目描述 ：
* 类 名 称 ：UnitDeviceDTO
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.DTO
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-09 13:36:01
* 更新时间 ：2019-03-09 13:36:01
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ Hany 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace YunWeiPingTai.DTO
{
    public class UnitDeviceDTO:BaseDTO
    {
        public long UnitId { get; set; }
        public string UnitName { get; set; }
        public long DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceVersion { get; set; }
        public string SNCode { get; set; }
        public long AddUserId { get; set; }
        public string AddUserName { get; set; }
    }
}