using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Data
{
    public partial class BookingContext : DbContext
    {
        public BookingContext()
        {
        }

        public BookingContext(DbContextOptions<BookingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Allergy> Allergy { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Note> Note { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<PatientAddress> PatientAddress { get; set; }
        public virtual DbSet<PatientAllergy> PatientAllergy { get; set; }
        public virtual DbSet<PatientPhoneNumber> PatientPhoneNumber { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumber { get; set; }
        public virtual DbSet<Religion> Religion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.HouseNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.StreetName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Town)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Allergy>(entity =>
            {
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Note)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_Note_Patient");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_Patient_Gender");

                entity.HasOne(d => d.Religion)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.ReligionId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Patient_Religion");
            });

            modelBuilder.Entity<PatientAddress>(entity =>
            {
                entity.HasKey(e => new { e.PatientId, e.AddressId });

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.PatientAddress)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_PatientAddress_Address");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientAddress)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_PatientAddress_Patient");
            });

            modelBuilder.Entity<PatientAllergy>(entity =>
            {
                entity.HasKey(e => new { e.PatientId, e.AllergyId });

                entity.HasOne(d => d.Allergy)
                    .WithMany(p => p.PatientAllergy)
                    .HasForeignKey(d => d.AllergyId)
                    .HasConstraintName("FK_PatientAllergy_Allergy");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientAllergy)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_PatientAllergy_Patient");
            });

            modelBuilder.Entity<PatientPhoneNumber>(entity =>
            {
                entity.HasKey(e => new { e.PatientId, e.PhoneNumberId });

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientPhoneNumber)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_PatientPhoneNumber_Patient");

                entity.HasOne(d => d.PhoneNumber)
                    .WithMany(p => p.PatientPhoneNumber)
                    .HasForeignKey(d => d.PhoneNumberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientPhoneNumber_PhoneNumber");
            });

            modelBuilder.Entity<PhoneNumber>(entity =>
            {
                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Religion>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
