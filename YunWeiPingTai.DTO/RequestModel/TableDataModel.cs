#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.DTO.RequestModel
* 项目描述 ：
* 类 名 称 ：TableDataModel
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.DTO.RequestModel
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-04-04 17:10:28
* 更新时间 ：2019-04-04 17:10:28
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ Hany 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace YunWeiPingTai.DTO.RequestModel
{
    public class TableDataModel
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int code { get; set; } = 0;
        /// <summary>
        /// 操作消息
        /// </summary>
        public string msg { get; set; } = "操作成功";

        /// <summary>
        /// 总记录条数
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 数据内容
        /// </summary>
        public dynamic data { get; set; }
    }
}
