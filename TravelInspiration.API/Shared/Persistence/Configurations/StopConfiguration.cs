using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;

namespace TravelInspiration.API.Shared.Persistence.Configurations;

public sealed class StopConfiguratio : IEntityTypeConfiguration<Stop>
{
    public void Configure(EntityTypeBuilder<Stop> builder)
    {
        builder.ToTable("Stops");
        builder.Property(e => e.Id).UseIdentityColumn();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.HasOne(s => s.Itinerary)
            .WithMany(i => i.Stops)
            .HasForeignKey(s => s.ItineraryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
