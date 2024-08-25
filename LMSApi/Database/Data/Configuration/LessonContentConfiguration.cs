﻿
using LMSApi.App.Enums;

namespace LMSApi.Database.Data.Configuration
{
    public class LessonContentConfiguration : IEntityTypeConfiguration<LessonContent>
    {
        public void Configure(EntityTypeBuilder<LessonContent> builder)
        {
            builder.HasKey(lc => lc.Id);
            builder.Property(lc => lc.Id).ValueGeneratedOnAdd();


            builder.Property(lc => lc.Name)
             .IsRequired()
             .HasColumnType("nvarchar(50)");

            //Enum
            //builder.Property(lc => lc.Type)
            //    .IsRequired()
            //    .HasConversion<LessonContentTypesEnum>();

            builder.Property(lc => lc.Link)
             .IsRequired()
             .HasColumnType("text");

            builder.Property(lc => lc.Content)
                .IsRequired()
                .HasColumnType("text");

            builder.HasOne(lc => lc.Lesson)
                .WithMany(l => l.LessonContents)
                .HasForeignKey(lc => lc.LessonId);
        }
    }
}
