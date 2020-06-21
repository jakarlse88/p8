using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CalHealth.CalendarService.Models;

namespace CalHealth.CalendarService.Data
{
    public partial class CalendarContext : DbContext
    {
        public CalendarContext()
        {
        }

        public CalendarContext(DbContextOptions<CalendarContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointment { get; set; }
        public virtual DbSet<Consultant> Consultant { get; set; }
        public virtual DbSet<ConsultantAvailabilityPeerWeek> ConsultantAvailabilityPeerWeek { get; set; }
        public virtual DbSet<Day> Day { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Specialty> Specialty { get; set; }
        public virtual DbSet<TimeSlot> TimeSlot { get; set; }
        public virtual DbSet<Week> Week { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasOne(d => d.Consultant)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.ConsultantId)
                    .HasConstraintName("FK_Appointment_Consultant");

                entity.HasOne(d => d.Day)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.DayId)
                    .HasConstraintName("FK_Appointment_Day");

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.TimeSlotId)
                    .HasConstraintName("FK_Appointment_TimeSlot");

                entity.HasOne(d => d.Week)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.WeekId)
                    .HasConstraintName("FK_Appointment_Week");
            });

            modelBuilder.Entity<Consultant>(entity =>
            {
                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Consultant)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consultant_Gender");

                entity.HasOne(d => d.Specialty)
                    .WithMany(p => p.Consultant)
                    .HasForeignKey(d => d.SpecialtyId)
                    .HasConstraintName("FK_Consultant_Specialty");
            });

            modelBuilder.Entity<ConsultantAvailabilityPeerWeek>(entity =>
            {
                entity.HasKey(e => new { e.ConsultantId, e.WeekId, e.DayId });

                entity.HasOne(d => d.Consultant)
                    .WithMany(p => p.ConsultantAvailabilityPeerWeek)
                    .HasForeignKey(d => d.ConsultantId)
                    .HasConstraintName("FK_ConsultantAvailabilityPeerWeek_Consultant");
            });

            modelBuilder.Entity<Day>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Specialty>(entity =>
            {
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
