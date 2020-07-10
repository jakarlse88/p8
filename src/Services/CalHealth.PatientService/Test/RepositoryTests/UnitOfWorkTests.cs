using System.Threading;
using System.Threading.Tasks;
using CalHealth.PatientService.Data;
using CalHealth.PatientService.Repositories;
using Moq;
using Xunit;

namespace CalHealth.PatientService.Test.RepositoryTests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void TestAllergyRepositoryProperty()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(Mock.Of<PatientContext>());

            // Act
            var result = unitOfWork.AllergyRepository;

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<AllergyRepository>(result);
        }
        
        [Fact]
        public void TestReligionRepositoryProperty()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(Mock.Of<PatientContext>());

            // Act
            var result = unitOfWork.ReligionRepository;

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ReligionRepository>(result);
        }
        
        [Fact]
        public void TestGenderRepositoryProperty()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(Mock.Of<PatientContext>());

            // Act
            var result = unitOfWork.GenderRepository;

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<GenderRepository>(result);
        }


        [Fact]
        public async Task TestCommitAsync()
        {
            // Arrange
            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Verifiable();

            var unitOfWork = new UnitOfWork(mockContext.Object);

            // Act
            await unitOfWork.CommitAsync();

            // Assert
            mockContext
                .Verify(x =>
                        x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task TestRollbackAsync()
        {
            // Arrange
            var mockContext = new Mock<PatientContext>();
            mockContext
                .Setup(x => x.DisposeAsync())
                .Verifiable();

            var unitOfWork = new UnitOfWork(mockContext.Object);

            // Act
            await unitOfWork.RollbackAsync();

            // Assert
            mockContext
                .Verify(x =>
                        x.DisposeAsync(),
                    Times.Once);
        }
    }
}