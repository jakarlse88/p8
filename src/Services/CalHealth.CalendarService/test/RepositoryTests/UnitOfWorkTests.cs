using System.Threading;
using System.Threading.Tasks;
using CalHealth.CalendarService.Data;
using CalHealth.CalendarService.Repositories;
using CalHealth.CalendarService.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CalHealth.CalendarService.Test.RepositoryTests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void TestTimeSlotRepository()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(Mock.Of<CalendarContext>());

            // Act
            var result = unitOfWork.TimeSlotRepository;

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ITimeSlotRepository>(result);
        }

        [Fact]
        public async Task TestCommitAsync()
        {
            // Arrange
            var mockContext = new Mock<CalendarContext>();
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
            var mockContext = new Mock<CalendarContext>();
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