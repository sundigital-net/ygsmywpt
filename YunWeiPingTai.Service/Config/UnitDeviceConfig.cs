#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.Service.Config
* 项目描述 ：
* 类 名 称 ：UnitDeviceConfig
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.Service.Config
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-09 9:44:00
* 更新时间 ：2019-03-09 9:44:00
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ Hany 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service.Config
{
    public class UnitDeviceConfig : IEntityTypeConfiguration<UnitDeviceEntity>
    {
        public void Configure(EntityTypeBuilder<UnitDeviceEntity> builder)
        {
            builder.ToTable("T_UnitDevices");
            builder.HasOne(t => t.Unit).WithMany().HasForeignKey(t => t.UnitId).IsRequired();
            builder.HasOne(t => t.AddUser).WithMany().HasForeignKey(t => t.AddUserId).IsRequired();
            builder.HasOne(t => t.Device).WithMany().HasForeignKey(t => t.DeviceId).IsRequired();
            builder.Property(t => t.SNCode).IsRequired().HasMaxLength(200);
        }
    }
}
