using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;

namespace TravelInspiration.API.Shared.Persistence.Configurations;

public sealed class ItineraryConfiguration : IEntityTypeConfiguration<Itinerary>
{
    public void Configure(EntityTypeBuilder<Itinerary> builder)
    {
        builder.ToTable("Itineraries");
        builder.Property(e => e.Id).UseIdentityColumn();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(2500);
        builder.Property(e => e.UserId).HasMaxLength(100).IsRequired();
        builder.HasMany(e => e.Stops)
            .WithOne(s => s.Itinerary)
            .HasForeignKey(s => s.ItineraryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
