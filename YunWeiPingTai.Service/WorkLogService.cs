using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using YunWeiPingTai.DTO;
using YunWeiPingTai.IService;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service
{
    class WorkLogService : IWorkLogService
    {
        private readonly MyDbContext _dbContext;
        public WorkLogService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public long AddNewLog(long userId, string logContent)
        {
            if(HasNewLog(userId))//今天已提交了日志
            {
                return -1;
            }
            var entity = new WorkLogEntity
            {
                IsRead = false,
                LogContent = logContent,
                UserId = userId
            };
            _dbContext.WorkLogs.Add(entity);
            _dbContext.SaveChanges();
            return entity.Id;

        }

        public long AddNewReply(long logId, string reply, long adminId)
        {
            var entity = new WorkLogReplyEntity
            {
                Reply = reply,
                WorkLogId = logId,
                UserId = adminId
            };
            _dbContext.WorkLogReplies.Add(entity);
            _dbContext.SaveChanges();
            return entity.Id;
        }

        private WorkLogDTO ToDTO(WorkLogEntity entity)
        {
            var dto = new WorkLogDTO
            {
                CreateTime = entity.CreateTime,
                Id = entity.Id,
                IsRead = entity.IsRead,
                LogContent = entity.LogContent,
                ReadTime = entity.ReadTime,
                UserId = entity.UserId,
                UserName = entity.User.Name
            };
            return dto;
        }
        public WorkLogDTO GetLogById(long id)
        {
            var log = _dbContext.WorkLogs.Include(t => t.User).SingleOrDefault(t => t.Id == id);
            return log == null ? null : ToDTO(log);
            //BaseService<WorkLogEntity> bs=new BaseService<WorkLogEntity>(_dbContext);
            //var log= bs.GetAll().Include(t => t.User).SingleOrDefault(t => t.Id == id);
            //return log == null ? null : ToDTO(log);
        }

        public WorkLogDTO[] GetLogs()
        {
            var logs = _dbContext.WorkLogs.OrderByDescending(t=>t.CreateTime).Include(t => t.User).ToList();
            var list=new List<WorkLogDTO>();
            foreach (var log in logs)
            {
                list.Add(ToDTO(log));
            }
            return list.ToArray();
        }

        public WorkLogDTO[] GetLogs(long userId)
        {
            var logs = _dbContext.WorkLogs.Where(t => t.UserId == userId).Include(t => t.User).OrderByDescending(t => t.CreateTime).ToList();
            var list = new List<WorkLogDTO>();
            foreach(var log in logs)
            {
                list.Add(ToDTO(log));
            }
            return list.ToArray();
        }

        private WorkLogReplyDTO ToDTO(WorkLogReplyEntity entity)
        {
            var dto = new WorkLogReplyDTO()
            {
                Id = entity.Id,
                AdminId = entity.UserId,
                AdminName = entity.User.Name,
                CreateTime = entity.CreateTime,
                Reply = entity.Reply,
                WorkLogId = entity.WorkLogId,
            };
            return dto;
        }
        public WorkLogReplyDTO[] GetReplies(long logId)
        {
            var replies = _dbContext.WorkLogReplies.Where(t => t.WorkLogId == logId).OrderByDescending(t => t.CreateTime).ToList();
            var list=new List<WorkLogReplyDTO>();
            foreach (var reply in replies)
            {
                list.Add(ToDTO(reply));
            }

            return list.ToArray();
        }

        public bool HasNewLog(long userId)
        {
            var log = _dbContext.WorkLogs.Where(t => t.UserId == userId && t.CreateTime.Date == DateTime.Now.Date);
            return log.Any();
        }

        public void DeleteLog(long id)
        {
            var log = _dbContext.WorkLogs.SingleOrDefault(t => t.Id == id);
            if(log==null)
                throw  new ArgumentException("不存在的日志信息，id="+id);
            log.IsDeleted = true;
            _dbContext.SaveChanges();
        }

        public void DeleteReply(long id)
        {
            var reply = _dbContext.WorkLogReplies.SingleOrDefault(t => t.Id == id);
            if (reply == null)
                throw new ArgumentException("不存在的日志回复信息，id=" + id);
            reply.IsDeleted = true;
            _dbContext.SaveChanges();
        }

        public WorkLogReplyDTO[] GetReplies()
        {
            var replies = _dbContext.WorkLogReplies.Include(t=>t.User).OrderByDescending(t => t.CreateTime).ToList();
            var list = new List<WorkLogReplyDTO>();
            foreach (var reply in replies)
            {
                list.Add(ToDTO(reply));
            }

            return list.ToArray();
        }
    }
}
