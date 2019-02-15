using System;
using System.Collections.Generic;
using System.Text;
using YunWeiPingTai.DTO;

namespace YunWeiPingTai.IService
{
    public interface IWorkLogService:IServiceSupport
    {
        long AddNewLog(long userId,string longContent);
        bool HasNewLog(long userId);
        WorkLogDTO[] GetLogs();
        WorkLogDTO[] GetLogs(long userId);
        WorkLogDTO GetLogById(long id);
        WorkLogReplyDTO[] GetReplies(long logId);
        WorkLogReplyDTO[] GetReplies();
        long AddNewReply(long logId, string reply, long adminId);
        void DeleteLog(long id);
        void DeleteReply(long id);
    }
}
