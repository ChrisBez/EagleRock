using EagleRock.Business;
using EagleRock.Cache;
using EagleRock.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace EagleRock.Testing
{
    public class EagleServiceTests
    {
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<EagleService>> _loggerMock;

        public EagleServiceTests()
        {
            _cacheServiceMock = new Mock<ICacheService>();

            _loggerMock = new Mock<ILogger<EagleService>>();
        }

        [Fact]
        public async void Service_Returns_True_And_Caches_If_Valid()
        {
            //Arrange
            _cacheServiceMock.Setup(cache => cache.AddEntryAsync(It.IsAny<TrafficData>()).Result).Returns(true);
            var sut = new EagleService(_loggerMock.Object, _cacheServiceMock.Object);
            var payload = GetValidPayload();

            //Act
            var result = await sut.StoreDataAsync(payload);

            //Assert
            Assert.True(result);
            _cacheServiceMock.Verify(c => c.AddEntryAsync(It.IsAny<TrafficData>()));
        }

        [Fact]
        public async void Service_Returns_False_If_CacheService_Fails()
        {
            //Arrange
            _cacheServiceMock.Setup(cache => cache.AddEntryAsync(It.IsAny<TrafficData>()).Result).Returns(false);
            var sut = new EagleService(_loggerMock.Object, _cacheServiceMock.Object);
            var payload = GetValidPayload();

            //Act
            var result = await sut.StoreDataAsync(payload);

            //Assert
            Assert.False(result);
            _cacheServiceMock.Verify(c => c.AddEntryAsync(It.IsAny<TrafficData>()));
        }

        [Theory]
        [InlineData(-27.623069255225083, 159.95153672634183, false)] //Too East
        [InlineData(1.9136021763806133, 139.0775144953842, false)] //Too North
        [InlineData(-24.06439390719994, 105.23962582625288, false)] //Too West
        [InlineData(-53.290091691726744, 131.43103038455357, false)] //Too South
        public async void Service_Rejects_Coords_Outside_Oz(double latitude, double longitude, bool expected)
        {
            //Arrange
            _cacheServiceMock.Setup(cache => cache.AddEntryAsync(It.IsAny<TrafficData>()).Result).Returns(true);
            var sut = new EagleService(_loggerMock.Object, _cacheServiceMock.Object);
            var payload = GetValidPayload();

            //overwrite lat long with ones for this theory
            payload.Latitude = latitude;
            payload.Longitude = longitude;

            //Act
            var result = await sut.StoreDataAsync(payload);

            //Assert
            Assert.Equal(result, expected);
            _cacheServiceMock.VerifyNoOtherCalls();

            //ILogger is messy, Serilog much easier to mock
            _loggerMock.Verify(x => x.Log(LogLevel.Warning,
               It.IsAny<EventId>(),
               It.IsAny<It.IsAnyType>(),
               It.IsAny<Exception>(),
               (Func<It.IsAnyType, Exception, string>)It.IsAny<object>())
            );
        }

        //TODO: Same style of tests for retrieval

        private TrafficData GetValidPayload()
        {
            return new TrafficData
            {
                BotId = System.Guid.Parse("00112233-4455-6677-8899-aabbccddeeff"),
                Latitude = -27.181719643914597,
                Longitude = 151.50579746842143,
                RoadName = "Main St",
                TrafficFlowDirection = FlowDirection.Outbound,
                TrafficFlowRate = 12.3,
                AverageVehicalSpeed = 40.3
            };
        }
        

    }
}