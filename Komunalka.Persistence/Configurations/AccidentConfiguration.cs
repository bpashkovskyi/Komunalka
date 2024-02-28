using Accidents.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Komunalka.Persistence.Configurations;

public class AccidentConfiguration : IEntityTypeConfiguration<Accident>
{
    public void Configure(EntityTypeBuilder<Accident> builder)
    {
        builder.OwnsOne(accident => accident.Address);
        builder.Property(accident => accident.Casualties).HasConversion(new ListToStringConverter());

        builder.Property(accident => accident.Reasons).HasConversion(new ListToStringConverter());

        builder.OwnsOne(accident => accident.Environment, ownedNavigationBuilder =>
        {
            ownedNavigationBuilder.Property(environment => environment.TrafficTools).HasConversion(new ListToStringConverter());
        });

        builder.OwnsOne(accident => accident.Coordinates);
    }
}

public class ListToStringConverter : ValueConverter<List<string>, string>
{
    public ListToStringConverter() : base(list => string.Join('|', list), // Convert to string for persistence
        @string => @string.Split('|', StringSplitOptions.None).ToList())
    {
    }
}