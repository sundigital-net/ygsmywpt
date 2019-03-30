#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.Service.Config
* 项目描述 ：
* 类 名 称 ：RoleMenuConfig
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.Service.Config
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-22 8:41:04
* 更新时间 ：2019-03-22 8:41:04
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
    public class RoleMenuConfig : IEntityTypeConfiguration<RoleMenuEntity>
    {
        public void Configure(EntityTypeBuilder<RoleMenuEntity> builder)
        {
            builder.ToTable("T_RoleMenus");
            builder.Property(t => t.RoleId).IsRequired();
            builder.Property(t => t.MenuId).IsRequired();
            builder.HasOne(t => t.Role).WithMany(t=>t.RoleMenus).HasForeignKey(t => t.RoleId).IsRequired();
            builder.HasOne(t => t.Menu).WithMany(t=>t.RoleMenus).HasForeignKey(t => t.MenuId).IsRequired();
        }
    }
}
