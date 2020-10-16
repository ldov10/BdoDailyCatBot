using System;
using BdoDailyCatBot.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BdoDailyCatBot
{
    public partial class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext()
        {
        }

        public DbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Captains> Captains { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(Secrets.dbToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Captains>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_Captains")
                    .IsUnique();

                entity.Property(e => e.LastDrivenRaid).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Captains)
                    .HasForeignKey<Captains>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Captains_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.IdDiscord).HasColumnType("numeric(20, 0)");

                entity.Property(e => e.LastRaidDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
