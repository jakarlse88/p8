using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalHealth.PatientService.Data;
using CalHealth.PatientService.Models;
using CalHealth.PatientService.Repositories;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CalHealth.PatientService.Test.RepositoryTests
{
    public class ReligionRepositoryTests
    {
        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var religions = new List<Religion>();
            var mockDbSet = religions.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Religion>())
                .Returns(mockDbSet.Object);

            var repository = new ReligionRepository(mockContext.Object);

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
            var religions = GenerateReligions();
            var mockDbSet = religions.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Religion>())
                .Returns(mockDbSet.Object);

            var repository = new ReligionRepository(mockContext.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
        
        /**
         * ============================
         * Internal helper methods
         *  ===========================
         */
        private static IEnumerable<Religion> GenerateReligions()
        {
            var genders = new List<Religion>
            {
                new Religion
                {
                    Id = 1
                },
                new Religion
                {
                    Id = 2
                }
            };

            return genders;
        }
    }
}