#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.Service.Config
* 项目描述 ：
* 类 名 称 ：DeviceConfig
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.Service.Config
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-09 9:06:03
* 更新时间 ：2019-03-09 9:06:03
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ Hany 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service.Config
{
    public class DeviceConfig : IEntityTypeConfiguration<DeviceEntity>
    {
        public void Configure(EntityTypeBuilder<DeviceEntity> builder)
        {
            builder.ToTable("T_Devices");
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Version).IsRequired().HasMaxLength(200);
        }
    }
}
