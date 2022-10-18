using IdServer.Core.CommandModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdServer.Infraestructure.Data.Command
{
    public class RoleConfiguration : BaseEntityConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);
            builder.HasKey(x => x.RoleCode);
            builder.Property(x => x.RoleCode).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(250);

        }
    }
}
