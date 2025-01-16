using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations
{
    public class UploadFileMorphConfiguration : IEntityTypeConfiguration<UploadFileMorph>
    {
        public void Configure(EntityTypeBuilder<UploadFileMorph> builder)
        {
            builder.Property(e => e.RelatedType).HasMaxLength(50);
            builder.Property(e => e.Field).HasMaxLength(50);
            builder.Property(e => e.RelatedType).HasDefaultValue(0);

            builder.HasOne<UploadFile>(x => x.UploadFile)
                .WithMany(x => x.UploadFileMorph)
                .HasForeignKey(x => x.UploadFileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
