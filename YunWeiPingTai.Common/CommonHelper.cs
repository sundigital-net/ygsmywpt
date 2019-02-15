using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace YunWeiPingTai.Common
{
    public class CommonHelper
    {
        public static string CalcMD5(byte[] bytes)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] computeBytes = md5.ComputeHash(bytes);
                string result = "";
                for (int i = 0; i < computeBytes.Length; i++)
                {
                    result += computeBytes[i].ToString("X").Length == 1 ? "0" + computeBytes[i].ToString("X") : computeBytes[i].ToString("X");
                }
                return result;
            }
        }
        public static string CalcMD5(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return CalcMD5(bytes);
        }
        /// <summary>
        /// 是否是邮箱
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool IsEmail(string account)
        {
            Regex regex=new Regex(@"([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,5})+");
            return regex.IsMatch(account);
        }
        /// <summary>
        /// 是否是手机号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool IsPhone(string account)
        {
            Regex regex=new Regex("^1[345789]\\d{9}$");
            return regex.IsMatch(account);
        }
    }
}
