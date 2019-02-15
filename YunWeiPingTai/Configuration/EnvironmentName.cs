using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YunWeiPingTai.Configuration
{
    /// <summary>
    /// 环境名称
    /// </summary>
    public class EnvironmentName
    {
        /// <summary>开发模式</summary>
        public static readonly string Development = nameof(Development);

        /// <summary>模拟环境</summary>
        public static readonly string Staging = nameof(Staging);

        /// <summary>测试环境</summary>
        public static readonly string Demo = nameof(Demo);

        /// <summary>生产环境</summary>
        public static readonly string Production = nameof(Production);
    }
}
