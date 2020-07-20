using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.PatientService.Messaging.Interfaces;
using CalHealth.PatientService.Messaging.Messages;
using CalHealth.PatientService.Models;
using CalHealth.PatientService.Models.MappingProfiles;
using CalHealth.PatientService.Repositories;
using Moq;
using Xunit;

namespace CalHealth.PatientService.Test.ServiceTests
{
    public class PatientServiceTests
    {
        private readonly IMapper _mapper;

        public PatientServiceTests()
        {
            var config = new MapperConfiguration(opt => opt.AddProfile(new PatientMappingProfile()));
            _mapper = new Mapper(config);    
        }
        
        [Fact]
        public async Task TestHandleIncomingPatientDataInvalidArgument()
        {
            // Arrange
            AppointmentMessage testMessage = null;

            var service = new Services.PatientService(null, null, null);

            // Act
            async Task TestAction() => await service.HandleIncomingPatientData(testMessage);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
        }

        [Fact]
        public async Task TestHandleIncomingPatientDataFirstNameNull()
        {
            // Arrange
            var testMessage = new AppointmentMessage
            {
                FirstName = null
            };

            var service = new Services.PatientService(null, null, null);

            // Act
            async Task TestAction() => await service.HandleIncomingPatientData(testMessage);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
        }

        [Fact]
        public async Task TestHandleIncomingPatientDataLastNameNull()
        {
            // Arrange
            var testMessage = new AppointmentMessage
            {
                FirstName = "Test",
                LastName = null
            };

            var service = new Services.PatientService(null, null, null);

            // Act
            async Task TestAction() => await service.HandleIncomingPatientData(testMessage);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
        }

        [Fact]
        public async Task TestHandleIncomingPatientData()
        {
            // Arrange
            var testMessage = new AppointmentMessage
            {
                FirstName = "Test",
                LastName = "McTest",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            var testPatient = new Patient
            {
                FirstName = testMessage.FirstName,
                LastName = testMessage.LastName,
                DateOfBirth = testMessage.DateOfBirth
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.PatientRepository.GetByConditionAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(new List<Patient> { testPatient })
                .Verifiable();

            var mockPublisher = new Mock<IPatientPublisher>();
            mockPublisher
                .Setup(x => x.PushMessageToQueue(It.IsAny<PatientMessage>()))
                .Verifiable();
            
            var service = new Services.PatientService(mockUnitOfWork.Object, mockPublisher.Object, null);

            // Act
            await service.HandleIncomingPatientData(testMessage);

            // Assert
            mockUnitOfWork
                .Verify(x => x.PatientRepository.GetByConditionAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<bool>()), Times.Once);

            mockPublisher
                .Verify(x => x.PushMessageToQueue(It.IsAny<PatientMessage>()), Times.Once);
        }

        [Fact]
        public async Task TestHandleIncomingDataPatientNull()
        {
            // Arrange
            var testMessage = new AppointmentMessage
            {
                FirstName = "Test",
                LastName = "McTest",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.PatientRepository.GetByConditionAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(new List<Patient>())
                .Verifiable();

            var mockPublisher = new Mock<IPatientPublisher>();
            mockPublisher
                .Setup(x => x.PushMessageToQueue(It.IsAny<PatientMessage>()))
                .Verifiable();
            
            var service = new Services.PatientService(mockUnitOfWork.Object, mockPublisher.Object, null);

            // Act
            async Task TestAction() => await service.HandleIncomingPatientData(testMessage);

            // Assert
            var ex = await Assert.ThrowsAnyAsync<Exception>(TestAction);
            Assert.Equal($"No {typeof(Patient)} entity matching the specified criteria exists.", ex.Message);
            
            mockUnitOfWork
                .Verify(x => x.PatientRepository.GetByConditionAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<bool>()),
                    Times.Once);
            
            mockPublisher
                .Verify(x => x.PushMessageToQueue(It.IsAny<PatientMessage>()), Times.Never);
        }

        [Fact]
        public async Task TestGetAllNoResults()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.PatientRepository.GetAllAsync(It.IsAny<bool>()))
                .ReturnsAsync(new List<Patient>());

            var service = new Services.PatientService(mockUnitOfWork.Object, null, _mapper);
            
            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<PatientDTO>>(result);
        }

        [Fact]
        public async Task TestGetAll()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.PatientRepository.GetAllAsync(It.IsAny<bool>()))
                .ReturnsAsync(new List<Patient>
                {
                    new Patient
                    {
                        Id = 1
                    },
                    new Patient
                    {
                        Id = 2
                    },
                    new Patient
                    {
                        Id = 3
                    }
                });

            var service = new Services.PatientService(mockUnitOfWork.Object, null, _mapper);
            
            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.IsAssignableFrom<IEnumerable<PatientDTO>>(result);
        }


    }
}