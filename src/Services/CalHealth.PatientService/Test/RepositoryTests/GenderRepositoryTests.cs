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
    public class GenderRepositoryTests
    {
        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var genders = new List<Gender>();
            var mockDbSet = genders.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Gender>())
                .Returns(mockDbSet.Object);

            var repository = new GenderRepository(mockContext.Object);

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
            var genders = GenerateGenders();
            var mockDbSet = genders.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Gender>())
                .Returns(mockDbSet.Object);

            var repository = new GenderRepository(mockContext.Object);

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
        private static IEnumerable<Gender> GenerateGenders()
        {
            var genders = new List<Gender>
            {
                new Gender
                {
                    Id = 1
                },
                new Gender
                {
                    Id = 2
                }
            };

            return genders;
        }
    }
}