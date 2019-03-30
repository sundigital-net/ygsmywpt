#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.Service.Entity
* 项目描述 ：
* 类 名 称 ：MenuEntity
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.Service.Entity
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-21 17:34:33
* 更新时间 ：2019-03-21 17:34:33
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
    public class MenuEntity:BaseEntity
    {
        public long ParentId { get; set; }
        public virtual MenuEntity ParentMenu { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string IconUrl { get; set; }
        public string LinkUrl { get; set; }
        public int? Sort { get; set; }
        public string Permission { get; set; }
        public virtual ICollection<RoleMenuEntity> RoleMenus { get; set; }=new List<RoleMenuEntity>();
    }
}
