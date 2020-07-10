using System.Threading;
using System.Threading.Tasks;
using CalHealth.BookingService.Data;
using CalHealth.BookingService.Repositories;
using Moq;
using Xunit;

namespace CalHealth.BookingService.Test.RepositoryTests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void TestTimeSlotRepositoryProperty()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(Mock.Of<BookingContext>());

            // Act
            var result = unitOfWork.TimeSlotRepository;

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ITimeSlotRepository>(result);
        }

        [Fact]
        public void TestConsultantRepositoryProperty()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(Mock.Of<BookingContext>());

            // Act
            var result = unitOfWork.ConsultantRepository;

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IConsultantRepository>(result);
        }


        [Fact]
        public async Task TestCommitAsync()
        {
            // Arrange
            var mockContext = new Mock<BookingContext>();
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
            var mockContext = new Mock<BookingContext>();
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