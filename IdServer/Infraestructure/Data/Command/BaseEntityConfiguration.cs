using IdServer.Core.CommandModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdServer.Infraestructure.Data.Command
{
    public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.Timestamp).IsRowVersion();
            builder.Property(e => e.CreatedBy).HasMaxLength(100).IsRequired();
            builder.Property(e => e.LastUpdatedBy).HasMaxLength(100);
        }
    }
}
