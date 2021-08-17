using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crossroad_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Crossroad_API.Database
{
    public class CrossroadDBContext: DbContext
    {
        public CrossroadDBContext(DbContextOptions<CrossroadDBContext> options) : base(options)
        {
        }

        public virtual DbSet<Award> Awards { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<OtherName> OtherNames { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<StoryLine> StoryLines { get; set; }
        public virtual DbSet<Title> Titles { get; set; }
        public virtual DbSet<TitleGenre> TitleGenres { get; set; }
        public virtual DbSet<TitleParticipant> TitleParticipants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Award>()
                .ToTable("Award", "dbo");

            modelBuilder.Entity<OtherName>()
                .ToTable("OtherName", "dbo");

            modelBuilder.Entity<Genre>()
            .ToTable("Genre", "dbo");

            modelBuilder.Entity<Participant>()
                .ToTable("Participant", "dbo");

            modelBuilder.Entity<StoryLine>()
               .ToTable("StoryLine", "dbo");

            modelBuilder.Entity<TitleGenre>()
                .ToTable("TitleGenre", "dbo");

            modelBuilder.Entity<TitleParticipant>()
                .ToTable("TitleParticipant", "dbo");

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.TitleGenres)
                .WithOne(e => e.Genre)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Participant>()
                .HasMany(e => e.TitleParticipants)
                .WithOne(e => e.Participant)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey(s => s.ParticipantId);
       
            modelBuilder.Entity<Title>()
                .ToTable("Title", "dbo")
                .HasMany(e => e.Awards)
                .WithOne(e => e.Title)                
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Title>()
                .ToTable("Title", "dbo")
                .HasMany(e => e.StoryLines)
                .WithOne(e => e.Title)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Title>()
                .ToTable("Title", "dbo")
                .HasMany(e => e.TitleGenres)
                .WithOne(e => e.Title)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Title>()
                .ToTable("Title", "dbo")
                .HasMany(e => e.TitleParticipants)
                .WithOne(e => e.Title)
                .OnDelete(DeleteBehavior.SetNull);
           
        }
    }
}
