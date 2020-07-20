using System;
using Microsoft.EntityFrameworkCore;
using CalHealth.BookingService.Models;
using System.Collections.Generic;

namespace CalHealth.BookingService.Data
{
    public class BookingContext : DbContext
    {
        public BookingContext()
        {
        }

        public BookingContext(DbContextOptions<BookingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointment { get; set; }
        public virtual DbSet<Consultant> Consultant { get; set; }
        public virtual DbSet<ConsultantAvailabilityPerWeek> ConsultantAvailabilityPerWeek { get; set; }
        public virtual DbSet<Day> Day { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Note> Note { get; set; }
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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointment_Day");

                entity.HasOne(d => d.Note)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.NoteId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Appointment_Note");

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.TimeSlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointment_TimeSlot");

                entity.HasOne(d => d.Week)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.WeekId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consultant_Specialty");
            });

            modelBuilder.Entity<ConsultantAvailabilityPerWeek>(entity =>
            {
                entity.HasKey(e => new { e.ConsultantId, e.WeekId, e.DayId });

                entity.HasOne(d => d.Consultant)
                    .WithMany(p => p.ConsultantAvailabilityPerWeek)
                    .HasForeignKey(d => d.ConsultantId)
                    .HasConstraintName("FK_ConsultantAvailabilityPerWeek_Consultant");

                entity.HasOne(d => d.Day)
                    .WithMany(p => p.ConsultantAvailabilityPerWeek)
                    .HasForeignKey(d => d.DayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsultantAvailabilityPerWeek_Day");

                entity.HasOne(d => d.Week)
                    .WithMany(p => p.ConsultantAvailabilityPerWeek)
                    .HasForeignKey(d => d.WeekId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsultantAvailabilityPerWeek_Week");
            });

            modelBuilder.Entity<Day>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.TimeCreated).HasColumnType("datetime");

                entity.Property(e => e.TimeLastUpdated).HasColumnType("datetime");
            });

            modelBuilder.Entity<Specialty>(entity =>
            {
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        private void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            PopulateGender(modelBuilder);
            PopulateDay(modelBuilder);
            PopulateWeek(modelBuilder);
            PopulateTimeSlot(modelBuilder);
            PopulateSpecialty(modelBuilder);
            PopulateConsultant(modelBuilder);
            PopulateConsultantAvailabilityPerWeek(modelBuilder);
        }

        private static void PopulateConsultantAvailabilityPerWeek(ModelBuilder modelBuilder)
        {
            const int consultantsTotal = 6;
            const int weeksTotal = 52;
            const int daysTotal = 7;

            var entities = new List<ConsultantAvailabilityPerWeek>();

            for (int consultant = 1; consultant <= consultantsTotal; consultant++)
            {
                for (int week = 1; week <= weeksTotal; week++)
                {
                    for (int day = 1; day <= daysTotal; day++)
                    {
                        var random = new Random();
                        var randomInt = random.Next(0, 100);

                        entities.Add(
                            new ConsultantAvailabilityPerWeek
                            {
                                ConsultantId = consultant,
                                WeekId = week,
                                DayId = day,
                                Available = randomInt >= 50
                            }
                        );
                    }
                }
            }

            modelBuilder.Entity<ConsultantAvailabilityPerWeek>().HasData(entities);
        }

        private static void PopulateConsultant(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Consultant>()
                .HasData(
                    new Consultant
                    {
                        Id = 1,
                        GenderId = 2,
                        SpecialtyId = 2,
                        FirstName = "Sophie",
                        LastName = "Harrington",
                        DateOfBirth = new DateTime(1985, 5, 24)
                    },
                    new Consultant
                    {
                        Id = 2,
                        GenderId = 1,
                        SpecialtyId = 5,
                        FirstName = "Kilian",
                        LastName = "Lopez",
                        DateOfBirth = new DateTime(1967, 2, 5)
                    },
                    new Consultant
                    {
                        Id = 3,
                        GenderId = 2,
                        SpecialtyId = 1,
                        FirstName = "Aya",
                        LastName = "Ahmed",
                        DateOfBirth = new DateTime(1990, 1, 9)
                    },
                    new Consultant
                    {
                        Id = 4,
                        GenderId = 2,
                        SpecialtyId = 2,
                        FirstName = "Hyeo-jin",
                        LastName = "Lim",
                        DateOfBirth = new DateTime(1980, 2, 29)
                    },
                    new Consultant
                    {
                        Id = 5,
                        GenderId = 1,
                        SpecialtyId = 7,
                        FirstName = "Lasse",
                        LastName = "Hansson",
                        DateOfBirth = new DateTime(1977, 12, 7)
                    },
                    new Consultant
                    {
                        Id = 6,
                        GenderId = 1,
                        SpecialtyId = 4,
                        FirstName = "Abe",
                        LastName = "Shiraishi",
                        DateOfBirth = new DateTime(1973, 9, 5)
                    }
                );
        }

        private static void PopulateSpecialty(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Specialty>()
                .HasData(
                    new Specialty
                    {
                        Id = 1,
                        Type = "Dermatology"
                    },
                    new Specialty
                    {
                        Id = 2,
                        Type = "Neurology"
                    },
                    new Specialty
                    {
                        Id = 3,
                        Type = "Pediatrics"
                    },
                    new Specialty
                    {
                        Id = 4,
                        Type = "Urology"
                    },
                    new Specialty
                    {
                        Id = 5,
                        Type = "Psychiatry"
                    },
                    new Specialty
                    {
                        Id = 6,
                        Type = "Obstetrics and Gynecology"
                    },
                    new Specialty
                    {
                        Id = 7,
                        Type = "Cardiology"
                    }
                );
        }

        private static void PopulateTimeSlot(ModelBuilder modelBuilder)
        {
            const int openingTime = 8;
            const int closingTime = 18;
            const int minutesPerHour = 60;
            const int sessionLength = 10;

            var timeSlots = new List<TimeSlot>();

            for (int hour = openingTime, index = 1; hour < closingTime; hour++)
            {
                for (int minute = 0; minute < (minutesPerHour / sessionLength); minute++)
                {
                    timeSlots.Add(
                        new TimeSlot
                        {
                            Id = index,
                            StartTime = new DateTime(2020, 1, 1, hour, (minute * 10), 0),
                            EndTime =
                                minute == 5
                                ? new DateTime(2020, 1, 1, (hour + 1), 0, 0)
                                : new DateTime(2020, 1, 1, hour, ((minute + 1) * 10), 0)
                        }
                    );

                    index++;
                }
            }

            modelBuilder
                .Entity<TimeSlot>()
                .HasData(timeSlots);
        }

        private static void PopulateWeek(ModelBuilder modelBuilder)
        {
            var weeks = new List<Week>();

            for (int i = 1; i <= 52; i++)
            {
                weeks.Add(new Week { Id = i, Number = (byte)i });
            }

            modelBuilder.Entity<Week>()
                .HasData(weeks);
        }

        private static void PopulateDay(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Day>()
                .HasData(new Day
                {
                    Id = 1,
                    Name = "Monday"
                },
                new Day
                {
                    Id = 2,
                    Name = "Tuesday"
                },
                new Day
                {
                    Id = 3,
                    Name = "Wednesday"
                },
                new Day
                {
                    Id = 4,
                    Name = "Thursday"
                },
                new Day
                {
                    Id = 5,
                    Name = "Friday"
                },
                new Day
                {
                    Id = 6,
                    Name = "Saturday"
                },
                new Day
                {
                    Id = 7,
                    Name = "Sunday"
                });
        }

        private static void PopulateGender(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>()
                .HasData(
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
    }
}
