using Microsoft.AspNetCore.Http;
using YunWeiPingTai.Common;

namespace YunWeiPingTai.Configuration
{
    public class SunDigitalSettings
    {
        public static readonly string DemoUserName = "sundigital@example.com";
        public static readonly string DemoPassword = "sundigital123";
        
        public string Environment { get; set; } = EnvironmentName.Development;
        public string UserName { get; }
    }
}
