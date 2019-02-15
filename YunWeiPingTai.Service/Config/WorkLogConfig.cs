using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service.Config
{
    public class WorkLogConfig : IEntityTypeConfiguration<WorkLogEntity>
    {
        public void Configure(EntityTypeBuilder<WorkLogEntity> builder)
        {
            builder.ToTable("T_WorkLogs");
            //builder.HasQueryFilter(t => !t.IsDeleted);
            builder.Property(t => t.LogContent).IsRequired();
            builder.HasOne(t => t.User).WithMany(t => t.WorkLogs).HasForeignKey(x => x.UserId);
        }
    }
}
