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
    public class AllergyRepositoryTests
    {
        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var allergies = new List<Allergy>();
            var mockDbSet = allergies.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Allergy>())
                .Returns(mockDbSet.Object);

            var repository = new AllergyRepository(mockContext.Object);

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
            var allergies = GenerateAllergies();
            var mockDbSet = allergies.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Allergy>())
                .Returns(mockDbSet.Object);

            var repository = new AllergyRepository(mockContext.Object);

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
        private static IEnumerable<Allergy> GenerateAllergies()
        {
            var genders = new List<Allergy>
            {
                new Allergy
                {
                    Id = 1
                },
                new Allergy
                {
                    Id = 2
                }
            };

            return genders;
        }
    }
}