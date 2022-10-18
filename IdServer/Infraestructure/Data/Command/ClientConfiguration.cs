using IdServer.Core.CommandModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdServer.Infraestructure.Data.Command
{
    public class ClientConfiguration : BaseEntityConfiguration<Client>
    {
        public override void Configure(EntityTypeBuilder<Client> builder)
        {
            base.Configure(builder);
            builder.HasKey(x => x.ClientId);
            builder.Property(x => x.ClientId).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(250);
            builder.Property(x => x.Scope).HasMaxLength(250);
        }
    }
}
