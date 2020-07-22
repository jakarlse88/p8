using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.BookingService.Messaging;
using CalHealth.BookingService.Messaging.Interfaces;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Models.MappingProfiles;
using CalHealth.BookingService.Repositories;
using CalHealth.BookingService.Services;
using Moq;
using Xunit;

namespace CalHealth.BookingService.Test.ServiceTests
{
    public class AppointmentServiceTests
    {
        private readonly IMapper _mapper;

        public AppointmentServiceTests()
        {
            var config = new MapperConfiguration(opt => opt.AddProfile(new AppointmentMappingProfile()));
            _mapper = new Mapper(config);
        }

        [Fact]
        public async Task TestCreateAsyncModelNull()
        {
            // Arrange
            var service = new AppointmentService(null, null, null);

            // Act
            async Task TestAction() => await service.CreateAsync(null);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
            Assert.Equal("model", ex.ParamName);
        }

        [Fact]
        public async Task TestCreateAsyncModelPatientNull()
        {
            // Arrange
            var service = new AppointmentService(null, null, null);

            // Act
            async Task TestAction() => await service.CreateAsync(new AppointmentDTO { Patient = null });

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
            Assert.Equal("model", ex.ParamName);
        }

        [Fact]
        public async Task TestCreateAsync()
        {
            // Arrange
            var consultant = new Consultant { Id = 1 };
            var week = new Week { Id = 1 };
            var day = new Day { Id = 1 };
            var timeSlot = new TimeSlot { Id = 1 };
            var dto = new AppointmentDTO
            {
                ConsultantId = consultant.Id,
                TimeSlotId = 1,
                Date = DateTime.Today,
                Patient = new PatientDTO()
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.ConsultantRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(consultant);

            mockUnitOfWork
                .Setup(x => x.WeekRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(week);

            mockUnitOfWork
                .Setup(x => x.DayRepository.GetByConditionAsync(It.IsAny<Expression<Func<Day, bool>>>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(new List<Day> { day });

            mockUnitOfWork
                .Setup(x => x.TimeSlotRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(timeSlot);

            mockUnitOfWork
                .Setup(x => x.AppointmentRepository.Add(It.IsAny<Appointment>()))
                .Verifiable();

            mockUnitOfWork
                .Setup(x => x.CommitAsync())
                .Verifiable();

            var mockPublisher = new Mock<IAppointmentPublisher>();
            mockPublisher
                .Setup(x => x.PushMessageToQueue(It.IsAny<AppointmentMessage>()))
                .Verifiable();

            var service = new AppointmentService(mockUnitOfWork.Object, mockPublisher.Object, _mapper);

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<AppointmentDTO>(result);

            mockPublisher
                .Verify(x => x.PushMessageToQueue(It.IsAny<AppointmentMessage>()), Times.Once());
            mockUnitOfWork
                .Verify(x => x.AppointmentRepository.Add(It.IsAny<Appointment>()), Times.Once);
            mockUnitOfWork
                .Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task TestCreateAsyncException()
        {
            // Arrange
            var consultant = new Consultant { Id = 1 };
            var week = new Week { Id = 1 };
            var day = new Day { Id = 1 };
            var timeSlot = new TimeSlot { Id = 1 };
            var dto = new AppointmentDTO
            {
                ConsultantId = consultant.Id,
                TimeSlotId = 1,
                Date = DateTime.Today,
                Patient = new PatientDTO()
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.ConsultantRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(consultant);

            mockUnitOfWork
                .Setup(x => x.WeekRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(week);

            mockUnitOfWork
                .Setup(x => x.DayRepository.GetByConditionAsync(It.IsAny<Expression<Func<Day, bool>>>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(new List<Day> { day });

            mockUnitOfWork
                .Setup(x => x.TimeSlotRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(timeSlot);

            mockUnitOfWork
                .Setup(x => x.AppointmentRepository.Add(It.IsAny<Appointment>()))
                .Verifiable();

            mockUnitOfWork
                .Setup(x => x.CommitAsync())
                .Verifiable();

            var mockPublisher = new Mock<IAppointmentPublisher>();
            mockPublisher
                .Setup(x => x.PushMessageToQueue(It.IsAny<AppointmentMessage>()))
                .Throws<Exception>()
                .Verifiable();

            var service = new AppointmentService(mockUnitOfWork.Object, mockPublisher.Object, _mapper);

            // Act
            async Task TestAction() => await service.CreateAsync(dto);

            // Assert
            await Assert.ThrowsAsync<Exception>(TestAction);

            mockPublisher
                .Verify(x => x.PushMessageToQueue(It.IsAny<AppointmentMessage>()), Times.Once());
            mockUnitOfWork
                .Verify(x => x.AppointmentRepository.Add(It.IsAny<Appointment>()), Times.Once);
            mockUnitOfWork
                .Verify(x => x.CommitAsync(), Times.Once);
        }


        [Fact]
        public async Task TestCreateAppointmentConflictWithExisting()
        {
            // Arrange
            var consultant = new Consultant { Id = 1 };
            var week = new Week { Id = 1 };
            var day = new Day { Id = 1 };
            var timeSlot = new TimeSlot { Id = 1 };
            var dto = new AppointmentDTO
            {
                ConsultantId = consultant.Id,
                TimeSlotId = 1,
                Date = DateTime.Today,
                Patient = new PatientDTO()
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.ConsultantRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(consultant);

            mockUnitOfWork
                .Setup(x => x.WeekRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(week);

            mockUnitOfWork
                .Setup(x => x.DayRepository.GetByConditionAsync(It.IsAny<Expression<Func<Day, bool>>>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(new List<Day> { day });

            mockUnitOfWork
                .Setup(x => x.TimeSlotRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .ReturnsAsync(timeSlot);

            mockUnitOfWork
                .Setup(x => x.AppointmentRepository.GetByConditionAsync(It.IsAny<Expression<Func<Appointment, bool>>>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(new List<Appointment>
                {
                    new Appointment
                    {
                        ConsultantId = consultant.Id,
                        TimeSlotId = 1,
                        WeekId = 1,
                        DayId = 1
                    }
                });

            mockUnitOfWork
                .Setup(x => x.AppointmentRepository.Add(It.IsAny<Appointment>()))
                .Verifiable();

            mockUnitOfWork
                .Setup(x => x.CommitAsync())
                .Verifiable();

            var mockPublisher = new Mock<IAppointmentPublisher>();
            mockPublisher
                .Setup(x => x.PushMessageToQueue(It.IsAny<AppointmentMessage>()))
                .Verifiable();

            var service = new AppointmentService(mockUnitOfWork.Object, mockPublisher.Object, _mapper);

            // Act
            async Task TestAction() => await service.CreateAsync(dto);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(TestAction);
            Assert.Equal("There already exists an appointment for this consultant at the specified time.", ex.Message);
        }

        [Fact]
        public async Task TestUpdatePatientIdAsyncAppointmentIdInvalid()
        {
            // Arrange
            var service = new AppointmentService(null, null, null);

            // Act
            async Task TestAction() => await service.UpdatePatientIdAsync(0, 0);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(TestAction);
            Assert.Equal("appointmentId", ex.ParamName);
        }

        [Fact]
        public async Task TestUpdatePatientIdAsyncAppointmentNotFound()
        {
            // Arrange
            const int testId = 1;
            var appointment = new Appointment { Id = 1 };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.AppointmentRepository.GetByIdAsync(1, It.IsAny<bool>()))
                .ReturnsAsync(null as Appointment);

            var service = new AppointmentService(mockUnitOfWork.Object, null, null);

            // Act
            async Task TestAction() => await service.UpdatePatientIdAsync(testId, 1);

            // Assert
            var ex = await Assert.ThrowsAsync<Exception>(TestAction);
            Assert.Equal(
                $"Received an unexpected null attempting to retrieve a {typeof(Appointment)} entity by the Id <{testId}>",
                ex.Message);
        }

        [Fact]
        public async Task TestUpdatePatientIdAsync()
        {
            // Arrange
            const int testEntityId = 1;
            const int testPatientId = 11;
            var testEntity = new Appointment { Id = 1 };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.AppointmentRepository.GetByIdAsync(1, It.IsAny<bool>()))
                .ReturnsAsync(testEntity);

            mockUnitOfWork
                .Setup(x => x.AppointmentRepository.Update(It.IsAny<Appointment>()))
                .Verifiable();

            mockUnitOfWork
                .Setup(x => x.CommitAsync())
                .Throws<Exception>();

            mockUnitOfWork
                .Setup(x => x.RollbackAsync())
                .Verifiable();

            var service = new AppointmentService(mockUnitOfWork.Object, null, null);

            // Act
            async Task TestAction() => await service.UpdatePatientIdAsync(testEntityId, testPatientId);

            // Assert
            await Assert.ThrowsAsync<Exception>(TestAction);

            mockUnitOfWork
                .Verify(x => x.AppointmentRepository.Update(It.IsAny<Appointment>()), Times.Once());
            mockUnitOfWork
                .Verify(x => x.CommitAsync(), Times.Once);
            mockUnitOfWork
                .Verify(x => x.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task TestGetByIdAsyncAppointmentIdInvalid()
        {
            // Arrange
            var service = new AppointmentService(null, null, null);

            // Act
            async Task TestAction() => await service.GetByIdAsync(0);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(TestAction);
            Assert.Equal("appointmentId", ex.ParamName);
        }

        [Fact]
        public async Task TestGetByIdAsyncEntityNotFound()
        {
            // Arrange
            const int testId = 1;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.AppointmentRepository.GetByIdAsync(testId, It.IsAny<bool>()))
                .ReturnsAsync(null as Appointment);

            var service = new AppointmentService(mockUnitOfWork.Object, null, null);

            // Act
            var result = await service.GetByIdAsync(testId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetByIdAsync()
        {
            // Arrange
            const int testId = 1;
            var appointment = new Appointment { Id = 1 };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.AppointmentRepository.GetByIdAsync(1, It.IsAny<bool>()))
                .ReturnsAsync(appointment);

            var service = new AppointmentService(mockUnitOfWork.Object, null, _mapper);

            // Act
            var result = await service.GetByIdAsync(testId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<AppointmentDTO>(result);
            Assert.Equal(testId, result.Id);
        }

        [Fact]
        public async Task TestGetByConsultantIdConsultantIdInvalid()
        {
            // Arrange
            var service = new AppointmentService(null, null, null);

            // Act
            async Task TestAction() => await service.GetByConsultantAsync(0);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(TestAction);
            Assert.Equal("consultantId", ex.ParamName);
        }

        [Fact]
        public async Task TestGetByConsultantId()
        {
            // Arrange
            const int testEntityId = 1;
            const int testConsultantId = 11;
            var testEntity = new Appointment { Id = testEntityId, ConsultantId = testConsultantId };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.AppointmentRepository.GetByConditionAsync(
                    It.IsAny<Expression<Func<Appointment, bool>>>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(new List<Appointment> { testEntity });
            
            var service = new AppointmentService(mockUnitOfWork.Object, null, _mapper);
            
            // Act
            var result = await service.GetByConsultantAsync(testConsultantId);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<AppointmentDTO>>(result);

        }
    }
}