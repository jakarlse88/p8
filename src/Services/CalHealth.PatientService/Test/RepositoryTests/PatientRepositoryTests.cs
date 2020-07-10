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
    public class PatientRepositoryTests
    {
        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var religions = new List<Patient>();
            var mockDbSet = religions.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.Set<Patient>())
                .Returns(mockDbSet.Object);

            var repository = new Repository<Patient>(mockContext.Object);

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
                .Setup(x => x.Set<Patient>())
                .Returns(mockDbSet.Object);

            var repository = new Repository<Patient>(mockContext.Object);

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
        private static IEnumerable<Patient> GenerateReligions()
        {
            var genders = new List<Patient>
            {
                new Patient
                {
                    Id = 1
                },
                new Patient
                {
                    Id = 2
                }
            };

            return genders;
        }
    }
}