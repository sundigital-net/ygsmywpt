using System;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Mailgun;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using YunWeiPingTai.Models;

namespace YunWeiPingTai.Services
{
    public class EmailSender:IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailOptions)
        {
            _emailSettings = emailOptions.Value;
        }
        /// <summary>
        /// 官方的demo
        /// </summary>
        /// <returns></returns>
        public IRestResponse SendEmail()
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri(_emailSettings.ApiBaseUri);
            client.Authenticator =
                new HttpBasicAuthenticator("api", _emailSettings.ApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain", _emailSettings.DomainName, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from",_emailSettings.From);
            request.AddParameter("to", "364572026@qq.com");
            request.AddParameter("subject", "Hello");
            request.AddParameter("text", "Testing some Mailgun awesomness!");
            request.Method = Method.POST;
            return client.Execute(request);
        }



        public Task SendEmailAsync(string account, string subject, string mess)
        {
            var domainName = _emailSettings.DomainName;
            var apiKey = _emailSettings.ApiKey;
            var sender = new MailgunSender(domainName, apiKey);
            Email.DefaultSender = sender;
            var email = Email.From(_emailSettings.From, "阳光数码运维平台")
                .To(account)
                .Subject(subject)
                .Body(mess);
            return email.SendAsync();
        }
    }
}
