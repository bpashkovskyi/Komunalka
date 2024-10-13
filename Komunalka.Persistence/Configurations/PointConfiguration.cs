using Light.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Komunalka.Persistence.Configurations;

public class PointConfiguration : IEntityTypeConfiguration<Point>
{
    public void Configure(EntityTypeBuilder<Point> builder)
    {
        builder.OwnsOne(point => point.Address);
        
        builder.OwnsOne(point => point.Coordinates);
    }
}