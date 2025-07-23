using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotifyHub.TelegramBot.Domain.Entities;

namespace NotifyHub.TelegramBot.Persistence.Configurations;

public class TelegramUserConfiguration: IEntityTypeConfiguration<TelegramUser>
{
    public void Configure(EntityTypeBuilder<TelegramUser> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(u => u.TelegramId)
            .HasColumnType("bigint");
    }
}