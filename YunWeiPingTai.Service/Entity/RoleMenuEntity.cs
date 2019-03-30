#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.Service.Entity
* 项目描述 ：
* 类 名 称 ：RoleMenuEntity
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.Service.Entity
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-22 8:30:21
* 更新时间 ：2019-03-22 8:30:21
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
     public class RoleMenuEntity:BaseEntity
    {
        public long RoleId { get; set; }
        public virtual RoleEntity Role { get; set; }
        public long MenuId { get; set; }
        public virtual MenuEntity Menu { get; set; }
    }
}
