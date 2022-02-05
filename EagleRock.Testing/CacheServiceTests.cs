using EagleRock.Cache;
using EagleRock.Models;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using System;
using Xunit;

namespace EagleRock.Testing
{
    public class CacheServiceTests
    {
        private readonly Mock<ILogger<CacheService>> _loggerMock;
        private readonly Mock<IDatabase> _databaseMock;
        private readonly Mock<IConnectionMultiplexer> _redisMock;

        public CacheServiceTests()
        {
            _redisMock = new Mock<IConnectionMultiplexer>();

            _loggerMock = new Mock<ILogger<CacheService>>();

            _databaseMock = new Mock<IDatabase>();
        }

        [Fact]
        public async void AddEntryAsync_Returns_True_And_Caches_If_Valid()
        {
            //Arrange
            _databaseMock.Setup(d => 
                d.StringSetAsync(
                    It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), null, It.IsAny<When>(), It.IsAny<CommandFlags>())
                ).ReturnsAsync(true);

            _redisMock
                .Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_databaseMock.Object);

            var sut = new CacheService(_redisMock.Object, _loggerMock.Object);

            var payload = GetValidPayload();

            //Act
            var result = await sut.AddEntryAsync(payload);

            //Assert
            Assert.True(result);
            
            _databaseMock.Verify(d => d.StringSetAsync(
                It.IsAny<RedisKey>(), It. IsAny<RedisValue>(), null, It.IsAny<When>(), It.IsAny<CommandFlags>()));
        }

        [Fact]
        public async void AddEntryAsync_Returns_False_And_Logs_If_Redis_Replies_False()
        {
            //Arrange
            _databaseMock.Setup(d =>
                d.StringSetAsync(
                    It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), null, It.IsAny<When>(), It.IsAny<CommandFlags>())
                .Result).Returns(false);

            _redisMock
                .Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_databaseMock.Object);

            var sut = new CacheService(_redisMock.Object, _loggerMock.Object);

            var payload = GetValidPayload();

            //Act
            var result = await sut.AddEntryAsync(payload);

            //Assert
            Assert.False(result);

            _databaseMock.Verify(d => d.StringSetAsync(
                It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), null, It.IsAny<When>(), It.IsAny<CommandFlags>()));

            //ILogger is messy to mock
            _loggerMock.Verify(x => x.Log(LogLevel.Warning,
               It.IsAny<EventId>(),
               It.IsAny<It.IsAnyType>(),
               It.IsAny<Exception>(),
               (Func<It.IsAnyType, Exception, string>)It.IsAny<object>())
            );
        }

        [Fact]
        public async void AddEntryAsync_Returns_False_And_Logs_If_Redis_Throws_Exception()
        {
            //Arrange
            _databaseMock.Setup(d =>
                d.StringSetAsync(
                    It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), null, It.IsAny<When>(), It.IsAny<CommandFlags>())
                .Result).Throws<Exception>();

            _redisMock
                .Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_databaseMock.Object);

            var sut = new CacheService(_redisMock.Object, _loggerMock.Object);

            var payload = GetValidPayload();

            //Act
            var result = await sut.AddEntryAsync(payload);

            //Assert
            Assert.False(result);

            _databaseMock.Verify(d => d.StringSetAsync(
                It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), null, It.IsAny<When>(), It.IsAny<CommandFlags>()));

            //ILogger is messy to mock
            _loggerMock.Verify(x => x.Log(LogLevel.Error,
               It.IsAny<EventId>(),
               It.IsAny<It.IsAnyType>(),
               It.IsAny<Exception>(),
               (Func<It.IsAnyType, Exception, string>)It.IsAny<object>())
            );
        }


        [Fact]
        public async void GetCurrentDataAsync_Returns_List_If_Valid()
        {
            //Arrange
            _databaseMock.Setup(d =>
                d.StringSetAsync(
                    It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), null, It.IsAny<When>(), It.IsAny<CommandFlags>())
                ).ReturnsAsync(true);

            _redisMock
                .Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_databaseMock.Object);

            var sut = new CacheService(_redisMock.Object, _loggerMock.Object);

            var payload = GetValidPayload();

            //Act
            var result = await sut.AddEntryAsync(payload);

            //Assert
            Assert.True(result);

            _databaseMock.Verify(d => d.StringSetAsync(
                It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), null, It.IsAny<When>(), It.IsAny<CommandFlags>()));
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
