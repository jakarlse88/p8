using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalHealth.CalendarService.Data;
using CalHealth.CalendarService.Models;
using CalHealth.CalendarService.Repositories;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CalHealth.CalendarService.Test.RepositoryTests
{
    public class ConsultantRepositoryTests
    {
        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var consultants = new List<Consultant>();
            var mockDbSet = consultants.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<CalendarContext>();
            mockContext
                .Setup(x => x.Set<Consultant>())
                .Returns(mockDbSet.Object);

            var repository = new ConsultantRepository(mockContext.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var consultants = GenerateConsultants();
            var mockDbSet = consultants.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<CalendarContext>();
            mockContext
                .Setup(x => x.Set<Consultant>())
                .Returns(mockDbSet.Object);

            var repository = new ConsultantRepository(mockContext.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count());
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
            };

            return consultants;
        }
    }
}