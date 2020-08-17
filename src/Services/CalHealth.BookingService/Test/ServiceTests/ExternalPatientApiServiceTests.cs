using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CalHealth.BookingService.Infrastructure;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace CalHealth.BookingService.Test.ServiceTests
{
    public class ExternalPatientApiServiceTests
    {
        private readonly IOptions<ExternalPatientApiOptions> _options;

        public ExternalPatientApiServiceTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            _options = Options.Create(configuration.GetSection("ExternalPatientApi").Get<ExternalPatientApiOptions>());
        }

        [Fact]
        public async Task TestPatientExistsArgumentNull()
        {
            // Arrange
            var service = new ExternalPatientApiService(Mock.Of<IMemoryCache>(), Mock.Of<IHttpClientFactory>(), Mock.Of<IOptions<ExternalPatientApiOptions>>(), Mock.Of<ILogger<ExternalPatientApiService>>());

            // Act
            async Task TestAction() => await service.PatientExists(null);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
        }

        [Theory]
        [InlineData("Test", "")]
        [InlineData("", "Test")]
        public async Task TestPatientExistsArgumentPropertyNull(string firstName, string lastName)
        {
            // Arrange
            var patient = new PatientDTO
            {
                FirstName = firstName,
                LastName = lastName
            };

            var cache = new MemoryCache(new MemoryCacheOptions());
            
            var mockLogger = new Mock<ILogger<ExternalPatientApiService>>();
            mockLogger.Setup(l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()))
                .Verifiable();
            
            var service = new ExternalPatientApiService(cache, Mock.Of<IHttpClientFactory>(), Mock.Of<IOptions<ExternalPatientApiOptions>>(), mockLogger.Object);

            // Act
            async Task TestAction() => await service.PatientExists(null);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAction);
        }

        [Fact]
        public async Task TestPatientExists()
        {
            // Arrange
            var testPatient = new PatientDTO
            {
                FirstName = "Test",
                LastName = "McTest",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            var cache = new MemoryCache(new MemoryCacheOptions());

            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(true.ToString()),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var expectedUri =
                new Uri(
                    $"https://localhost:5003/api/patient/exists?firstName={testPatient.FirstName}&lastName={testPatient.LastName}&dateOfBirth={testPatient.DateOfBirth.ToShortDateString()}");

            var mockLogger = new Mock<ILogger<ExternalPatientApiService>>();
            mockLogger.Setup(l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()))
                .Verifiable();

            var service = new ExternalPatientApiService(cache, mockFactory.Object, _options, mockLogger.Object);

            // Act
            var result = await service.PatientExists(testPatient);

            // Assert
            Assert.True(result);

            mockHttpMessageHandler
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == expectedUri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

        [Fact]
        public async Task TestPatientExistsNotFound()
        {
            // Arrange
            var testPatient = new PatientDTO
            {
                FirstName = "Test",
                LastName = "McTest",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            var cache = new MemoryCache(new MemoryCacheOptions());
            
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{true}"),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            
            var mockLogger = new Mock<ILogger<ExternalPatientApiService>>();
            mockLogger.Setup(l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()))
                .Verifiable();

            var expectedUri =
                new Uri(
                    $"https://localhost:5003/api/patient/exists?firstName={testPatient.FirstName}&lastName={testPatient.LastName}&dateOfBirth={testPatient.DateOfBirth.ToShortDateString()}");

            var service = new ExternalPatientApiService(cache, mockFactory.Object, _options, mockLogger.Object);

            // Act
            var result = await service.PatientExists(testPatient);

            // Assert
            Assert.False(result);

            mockHttpMessageHandler
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == expectedUri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
    }
}