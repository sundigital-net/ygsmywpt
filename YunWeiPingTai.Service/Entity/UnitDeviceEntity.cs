#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.Service.Entity
* 项目描述 ：
* 类 名 称 ：UnitDeviceRelationEntity
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.Service.Entity
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-09 9:38:52
* 更新时间 ：2019-03-09 9:38:52
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
    public class UnitDeviceEntity:BaseEntity
    {
        public long UnitId { get; set; }
        public virtual UnitEntity Unit { get; set; }
        public long DeviceId { get; set; }
        public virtual DeviceEntity Device { get; set; }
        public string SNCode { get; set; }//唯一识别编码
        public long AddUserId { get; set; }//添加人
        public virtual UserEntity AddUser { get; set; }
    }
}
