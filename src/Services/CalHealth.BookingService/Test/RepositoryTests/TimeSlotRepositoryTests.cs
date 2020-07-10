using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalHealth.BookingService.Data;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Repositories;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CalHealth.BookingService.Test.RepositoryTests
{
    public class TimeSlotRepositoryTests
    {
        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var timeSlots = new List<TimeSlot>();
            var mockDbSet = timeSlots.AsQueryable().BuildMockDbSet();
            
            var mockContext = new Mock<BookingContext>();
            mockContext
                .Setup(x => x.Set<TimeSlot>())
                .Returns(mockDbSet.Object);

            var repository = new TimeSlotRepository(mockContext.Object); 
            
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
            var timeSlots = GenerateTimeSlots();
            var mockDbSet = timeSlots.AsQueryable().BuildMockDbSet();
            
            var mockContext = new Mock<BookingContext>();
            mockContext
                .Setup(x => x.Set<TimeSlot>())
                .Returns(mockDbSet.Object);

            var repository = new TimeSlotRepository(mockContext.Object); 
            
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

        private IEnumerable<TimeSlot> GenerateTimeSlots()
        {
            const int openingTime = 8;
            const int closingTime = 9;
            const int minutesPerHour = 60;
            const int sessionLength = 10;

            var timeSlots = new List<TimeSlot>();

            for (int hour = openingTime, index = 1; hour < closingTime; hour++)
            {
                for (int minute = 0; minute < (minutesPerHour / sessionLength); minute++)
                {
                    timeSlots.Add(
                        new TimeSlot
                        {
                            Id = index,
                            StartTime = new DateTime(2020, 1, 1, hour, (minute * 10), 0),
                            EndTime =
                                minute == 5
                                    ? new DateTime(2020, 1, 1, (hour + 1), 0, 0)
                                    : new DateTime(2020, 1, 1, hour, ((minute + 1) * 10), 0)
                        }
                    );

                    index++;
                }
            }

            return timeSlots;
        }
    }
}