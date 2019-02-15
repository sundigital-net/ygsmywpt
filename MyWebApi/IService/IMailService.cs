using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.IService
{
    public interface IMailService
    {
        void Send(string subject, string msg);
    }
}
