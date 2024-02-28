using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Protocols.Model;

namespace Komunalka.Persistence.Configurations;

public class ProtocolConfiguration : IEntityTypeConfiguration<Protocol>
{
    public void Configure(EntityTypeBuilder<Protocol> builder)
    {
        builder.HasMany(protocol => protocol.Items)
            .WithOne(item => item.Protocol);
    }
}