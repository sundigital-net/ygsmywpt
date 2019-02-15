using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service.Config
{
    public class WorkLogReplyConfig : IEntityTypeConfiguration<WorkLogReplyEntity>
    {
        public void Configure(EntityTypeBuilder<WorkLogReplyEntity> builder)
        {
            builder.ToTable("T_WorkLogReplies");
            //builder.HasQueryFilter(t => !t.IsDeleted);
            builder.Property(t => t.Reply).IsRequired().HasMaxLength(1024);
            //builder.HasOne(t => t.WorkLog).WithMany(t => t.WorkLogReplies).HasForeignKey(t => t.WorkLogId);
            //builder.HasOne(t => t.User).WithMany(t => t.WorkLogReplies).HasForeignKey(t => t.UserId);
        }
    }
}
