using IdServer.Core.CommandModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdServer.Infraestructure.Data.Command
{
    public class ClientUserRoleConfiguration : BaseEntityConfiguration<ClientUserRole>
    {
        public override void Configure(EntityTypeBuilder<ClientUserRole> builder)
        {
            base.Configure(builder);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RoleCode).HasMaxLength(50);
            builder.Property(x => x.ClientId).HasMaxLength(50);
        }
    }
}
