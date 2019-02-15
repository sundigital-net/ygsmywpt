using RestSharp;
using System.Threading.Tasks;

namespace YunWeiPingTai.Services
{
    public interface IEmailSender
    {
        IRestResponse SendEmail();
        Task SendEmailAsync(string email, string subject, string message);
    }
}
