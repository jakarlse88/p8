using System;
using CalHealth.PatientService.Models;
using Microsoft.EntityFrameworkCore;

namespace CalHealth.PatientService.Data
{
    public class PatientContext : DbContext
    {
        public PatientContext()
        {
        }

        public PatientContext(DbContextOptions<PatientContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Allergy> Allergy { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<PatientAddress> PatientAddress { get; set; }
        public DbSet<PatientAllergy> PatientAllergy { get; set; }
        public DbSet<PatientPhoneNumber> PatientPhoneNumber { get; set; }
        public DbSet<PhoneNumber> PhoneNumber { get; set; }
        public DbSet<Religion> Religion { get; set; }

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
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasIndex(e => e.GenderId);

                entity.HasIndex(e => e.ReligionId);

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

                entity.HasIndex(e => e.AddressId);

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

                entity.HasIndex(e => e.AllergyId);

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

                entity.HasIndex(e => e.PhoneNumberId);

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

        private void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            PopulateAllergy(modelBuilder);
            PopulateReligion(modelBuilder);
            PopulateGender(modelBuilder);
            PopulatePatient(modelBuilder);    
        }

        /**
         *
         * Private helper methods
         * 
         */
        private static void PopulateGender(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>().HasData(
                new Gender
                {
                    Id = 1,
                    Type = "Male"
                },
                new Gender
                {
                    Id = 2,
                    Type = "Female"
                }
            );
        }

        private static void PopulateReligion(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Religion>().HasData(
                new Religion
                {
                    Id = 1,
                    Name = "Christianity (Protestant)"
                },
                new Religion
                {
                    Id = 2,
                    Name = "Christianity (Roman Catholic)"
                },
                new Religion
                {
                    Id = 3,
                    Name = "Christianity (Orthodox)"
                },
                new Religion
                {
                    Id = 4,
                    Name = "Islam (Shia)"
                },
                new Religion
                {
                    Id = 5,
                    Name = "Islam (Sunni)"
                },
                new Religion
                {
                    Id = 6,
                    Name = "Judaism"
                },
                new Religion
                {
                    Id = 7,
                    Name = "Buddhism"
                },
                new Religion
                {
                    Id = 8,
                    Name = "Hinduism"
                },
                new Religion
                {
                    Id = 9,
                    Name = "Scientology"
                }
            );
        }

        private static void PopulateAllergy(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Allergy>().HasData(
                new Allergy
                {
                    Id = 1,
                    Type = "Latex"
                },
                new Allergy
                {
                    Id = 2,
                    Type = "Nuts"
                },
                new Allergy
                {
                    Id = 3,
                    Type = "Fruit"
                },
                new Allergy
                {
                    Id = 4,
                    Type = "Shellfish"
                },
                new Allergy
                {
                    Id = 5,
                    Type = "Egg"
                },
                new Allergy
                {
                    Id = 6,
                    Type = "Lactose"
                },
                new Allergy
                {
                    Id = 7,
                    Type = "Mould"
                },
                new Allergy
                {
                    Id = 8,
                    Type = "Antibiotics"
                }
            );
        }

        private static void PopulatePatient(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasData(
                    new Patient
                    {
                        Id = 1,
                        FirstName = "Alima",
                        LastName = "Rankin",
                        GenderId = 2,
                        DateOfBirth = new DateTime(2000, 1, 1)
                    },
                    new Patient
                    {
                        Id = 2,
                        FirstName = "Chelsie",
                        LastName = "Regan",
                        GenderId = 2,
                        DateOfBirth = new DateTime(1980, 7, 17)
                    },
                    new Patient
                    {
                        Id = 3,
                        FirstName = "Michalina",
                        LastName = "Dejesus",
                        GenderId = 2,
                        DateOfBirth = new DateTime(1997, 3, 12)
                    },
                    new Patient
                    {
                        Id = 4,
                        FirstName = "Daniaal",
                        LastName = "Hill",
                        GenderId = 1,
                        DateOfBirth = new DateTime(2007, 5, 27)
                    },
                    new Patient
                    {
                        Id = 5,
                        FirstName = "Adele",
                        LastName = "Benjamin",
                        GenderId = 2,
                        DateOfBirth = new DateTime(1989, 11, 29)
                    },
                    new Patient
                    {
                        Id = 6,
                        FirstName = "Rhodri",
                        LastName = "Ellis",
                        GenderId = 1,
                        DateOfBirth = new DateTime(1977, 9, 9)
                    },
                    new Patient
                    {
                        Id = 7,
                        FirstName = "Hakeem",
                        LastName = "Conner",
                        GenderId = 1,
                        DateOfBirth = new DateTime(2001, 4, 11)
                    },
                    new Patient
                    {
                        Id = 8,
                        FirstName = "Nur",
                        LastName = "Lim",
                        GenderId = 1,
                        DateOfBirth = new DateTime(1963, 02, 19)
                    },
                    new Patient
                    {
                        Id = 9,
                        FirstName = "Kenzo",
                        LastName = "Traynor",
                        GenderId = 1,
                        DateOfBirth = new DateTime(1990, 10, 30)
                    },
                    new Patient
                    {
                        Id = 10,
                        FirstName = "Nyla",
                        LastName = "Davey",
                        GenderId = 2,
                        DateOfBirth = new DateTime(2007, 06, 13)
                    });
        }
    }
}
