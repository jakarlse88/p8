﻿// <auto-generated />
using System;
using CalHealth.BookingService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CalHealth.BookingService.Data.Migrations
{
    [DbContext(typeof(BookingContext))]
    partial class BookingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CalHealth.BookingService.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HouseNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Town")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("CalHealth.BookingService.Models.Allergy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Allergy");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "Latex"
                        },
                        new
                        {
                            Id = 2,
                            Type = "Nuts"
                        },
                        new
                        {
                            Id = 3,
                            Type = "Fruit"
                        },
                        new
                        {
                            Id = 4,
                            Type = "Shellfish"
                        },
                        new
                        {
                            Id = 5,
                            Type = "Egg"
                        },
                        new
                        {
                            Id = 6,
                            Type = "Lactose"
                        },
                        new
                        {
                            Id = 7,
                            Type = "Mould"
                        },
                        new
                        {
                            Id = 8,
                            Type = "Antibiotics"
                        });
                });

            modelBuilder.Entity("CalHealth.BookingService.Models.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Gender");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "Male"
                        },
                        new
                        {
                            Id = 2,
                            Type = "Female"
                        });
                });

            modelBuilder.Entity("CalHealth.BookingService.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("GenderId")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int?>("ReligionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GenderId");

                    b.HasIndex("ReligionId");

                    b.ToTable("Patient");
                });

            modelBuilder.Entity("CalHealth.BookingService.Models.PatientAddress", b =>
                {
                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.HasKey("PatientId", "AddressId");

                    b.HasIndex("AddressId");

                    b.ToTable("PatientAddress");
                });

            modelBuilder.Entity("CalHealth.BookingService.Models.PatientAllergy", b =>
                {
                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("AllergyId")
                        .HasColumnType("int");

                    b.HasKey("PatientId", "AllergyId");

                    b.HasIndex("AllergyId");

                    b.ToTable("PatientAllergy");
                });

            modelBuilder.Entity("CalHealth.BookingService.Models.PatientPhoneNumber", b =>
                {
                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("PhoneNumberId")
                        .HasColumnType("int");

                    b.HasKey("PatientId", "PhoneNumberId");

                    b.HasIndex("PhoneNumberId");

                    b.ToTable("PatientPhoneNumber");
                });

            modelBuilder.Entity("CalHealth.BookingService.Models.PhoneNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("varchar(12)")
                        .HasMaxLength(12)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("PhoneNumber");
                });

            modelBuilder.Entity("CalHealth.BookingService.Models.Religion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(60)")
                        .HasMaxLength(60)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Religion");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Christianity (Protestant)"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Christianity (Roman Catholic)"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Christianity (Orthodox)"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Islam (Shia)"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Islam (Sunni)"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Judaism"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Buddhism"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Hinduism"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Scientology"
                        });
                });

            modelBuilder.Entity("CalHealth.BookingService.Models.Patient", b =>
                {
                    b.HasOne("CalHealth.BookingService.Models.Gender", "Gender")
                        .WithMany("Patient")
                        .HasForeignKey("GenderId")
                        .HasConstraintName("FK_Patient_Gender")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CalHealth.BookingService.Models.Religion", "Religion")
                        .WithMany("Patient")
                        .HasForeignKey("ReligionId")
                        .HasConstraintName("FK_Patient_Religion")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("CalHealth.BookingService.Models.PatientAddress", b =>
                {
                    b.HasOne("CalHealth.BookingService.Models.Address", "Address")
                        .WithMany("PatientAddress")
                        .HasForeignKey("AddressId")
                        .HasConstraintName("FK_PatientAddress_Address")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CalHealth.BookingService.Models.Patient", "Patient")
                        .WithMany("PatientAddress")
                        .HasForeignKey("PatientId")
                        .HasConstraintName("FK_PatientAddress_Patient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CalHealth.BookingService.Models.PatientAllergy", b =>
                {
                    b.HasOne("CalHealth.BookingService.Models.Allergy", "Allergy")
                        .WithMany("PatientAllergy")
                        .HasForeignKey("AllergyId")
                        .HasConstraintName("FK_PatientAllergy_Allergy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CalHealth.BookingService.Models.Patient", "Patient")
                        .WithMany("PatientAllergy")
                        .HasForeignKey("PatientId")
                        .HasConstraintName("FK_PatientAllergy_Patient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CalHealth.BookingService.Models.PatientPhoneNumber", b =>
                {
                    b.HasOne("CalHealth.BookingService.Models.Patient", "Patient")
                        .WithMany("PatientPhoneNumber")
                        .HasForeignKey("PatientId")
                        .HasConstraintName("FK_PatientPhoneNumber_Patient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CalHealth.BookingService.Models.PhoneNumber", "PhoneNumber")
                        .WithMany("PatientPhoneNumber")
                        .HasForeignKey("PhoneNumberId")
                        .HasConstraintName("FK_PatientPhoneNumber_PhoneNumber")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
