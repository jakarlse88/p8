using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Models.MappingProfiles;
using CalHealth.BookingService.Repositories;
using CalHealth.BookingService.Services;
using Moq;
using Xunit;

namespace CalHealth.BookingService.Test.ServiceTests
{
    public class ConsultantServiceTests
    {
        private readonly IMapper _mapper;

        public ConsultantServiceTests()
        {
            var config = new MapperConfiguration(opt => { opt.AddProfile(new ConsultantMappingProfile()); });

            _mapper = config.CreateMapper();
        }
        
        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var consultants = GenerateConsultants();
            
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.ConsultantRepository.GetAllAsync(It.IsAny<bool>()))
                .ReturnsAsync(consultants);

            var service = new ConsultantService(mockUnitOfWork.Object, _mapper);
            
            // Act
            var result = await service.GetAllAsDTOAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());
        }
        
        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var consultants = new List<Consultant>();
            
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.ConsultantRepository.GetAllAsync(It.IsAny<bool>()))
                .ReturnsAsync(consultants);

            var service = new ConsultantService(mockUnitOfWork.Object, _mapper);
            
            // Act
            var result = await service.GetAllAsDTOAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */

        private static IEnumerable<Consultant> GenerateConsultants()
        {
            var consultants = new List<Consultant>
            {
                new Consultant
                {
                    Id = 1,
                    GenderId = 2,
                    SpecialtyId = 2,
                    FirstName = "Sophie",
                    LastName = "Harrington",
                    DateOfBirth = new DateTime(1985, 5, 24),
                    Specialty =  new Specialty
                    {
                        Type = "Test"
                    }
                },
                new Consultant
                {
                    Id = 2,
                    GenderId = 1,
                    SpecialtyId = 5,
                    FirstName = "Kilian",
                    LastName = "Lopez",
                    DateOfBirth = new DateTime(1967, 2, 5),
                    Specialty =  new Specialty
                    {
                        Type = "Test"
                    }
                },
                new Consultant
                {
                    Id = 3,
                    GenderId = 2,
                    SpecialtyId = 1,
                    FirstName = "Aya",
                    LastName = "Ahmed",
                    DateOfBirth = new DateTime(1990, 1, 9),
                    Specialty =  new Specialty
                    {
                        Type = "Test"
                    }
                },
                new Consultant
                {
                    Id = 4,
                    GenderId = 2,
                    SpecialtyId = 2,
                    FirstName = "Hyeo-jin",
                    LastName = "Lim",
                    DateOfBirth = new DateTime(1980, 2, 29),
                    Specialty =  new Specialty
                    {
                        Type = "Test"
                    }
                },
                new Consultant
                {
                    Id = 5,
                    GenderId = 1,
                    SpecialtyId = 7,
                    FirstName = "Lasse",
                    LastName = "Hansson",
                    DateOfBirth = new DateTime(1977, 12, 7),
                    Specialty =  new Specialty
                    {
                        Type = "Test"
                    }
                },
                new Consultant
                {
                    Id = 6,
                    GenderId = 1,
                    SpecialtyId = 4,
                    FirstName = "Abe",
                    LastName = "Shiraishi",
                    DateOfBirth = new DateTime(1973, 9, 5),
                    Specialty =  new Specialty
                    {
                        Type = "Test"
                    }
                }
            };

            return consultants;
        }
    }
}