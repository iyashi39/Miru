// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using Autofac;
using Autofac.Extras.Moq;
using MiruLibrary.Models;
using MiruLibrary.Services;
using Moq;
using MyInternetConnectionLibrary.Anime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Miru.Tests.ModelsTests
{
    public class CurrentSeasonModelTests
    {
        [Fact]
        public async Task GetCurrentSeasonList_OnHttpRequestException_ReturnsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                mock.Mock<IAnimeApiClient>()
                    .Setup(x => x.GetCurrentSeasonAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new HttpRequestException());
                var sut = mock.Create<CurrentSeasonModel>();

                // Act
                var result = await sut.GetCurrentSeasonList(It.IsAny<int>());

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task GetCurrentSeasonList_OnSeasonDataGet_ReturnsTrue()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                mock.Mock<IAnimeApiClient>()
                    .Setup(x => x.GetCurrentSeasonAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new AnimePageEnvelope { Data = new List<AnimeDto>() });
                var sut = mock.Create<CurrentSeasonModel>();

                // Act
                var result = await sut.GetCurrentSeasonList(It.IsAny<int>());

                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task GetCurrentSeasonList_OnMultiplePages_GetsAllOfThem()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                mock.Mock<IAnimeApiClient>()
                    .SetupSequence(x => x.GetCurrentSeasonAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new AnimePageEnvelope
                    {
                        Data = new List<AnimeDto> { new AnimeDto { MalId = 1 } },
                        Pagination = new PaginationDto { HasNextPage = true }
                    })
                    .ReturnsAsync(new AnimePageEnvelope
                    {
                        Data = new List<AnimeDto> { new AnimeDto { MalId = 2 } },
                        Pagination = new PaginationDto { HasNextPage = false }
                    });
                var sut = mock.Create<CurrentSeasonModel>();

                // Act
                var result = await sut.GetCurrentSeasonList(0);

                // Assert
                Assert.True(result);
                Assert.Equal(2, sut.SeasonData.Data.Count);
            }
        }

        [Fact]
        public void GetCurrentSeasonList_OnAnimeApiException_DelaysBeforeRetry()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                mock.Mock<IAnimeApiClient>()
                    .SetupSequence(x => x.GetCurrentSeasonAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new AnimeApiException("error"))
                    .ReturnsAsync(new AnimePageEnvelope { Data = new List<AnimeDto>() });
                mock.Mock<ITimerService>().Setup(x => x.DelayTask(It.IsAny<int>())).Returns(Task.CompletedTask);

                var sut = mock.Create<CurrentSeasonModel>();
                int testDelayInMs = 1;

                // Act
                sut.GetCurrentSeasonList(testDelayInMs).Wait();

                // Assert
                mock.Mock<ITimerService>().Verify(x => x.DelayTask(testDelayInMs), Times.AtLeastOnce());
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetFilteredSeasonList_ReturnsSeasonListWithoutAnimeForKids(bool forKids)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testSeasonData = new AnimePageEnvelope
                {
                    Data = new List<AnimeDto>()
                    {
                        CreateSeasonEntry(forKids),
                        CreateSeasonEntry(false),
                        CreateSeasonEntry(false),
                        CreateSeasonEntry(forKids),
                        CreateSeasonEntry(false),
                        CreateSeasonEntry(false),
                    }
                };

                mock.Mock<IAnimeApiClient>()
                    .Setup(x => x.GetCurrentSeasonAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(testSeasonData);
                var sut = mock.Create<CurrentSeasonModel>();
                sut.GetCurrentSeasonList(0).Wait();

                var expectedResult = testSeasonData.Data.ToList();
                expectedResult.RemoveAll(x => CurrentSeasonModel.IsForKids(x));

                // Act
                var result = sut.GetFilteredSeasonList();

                // Assert
                Assert.True(result.Count == expectedResult.Count);
                Assert.Equal(expectedResult, result);
            }
        }

        private static AnimeDto CreateSeasonEntry(bool forKids)
        {
            return new AnimeDto
            {
                Demographics = forKids
                    ? new List<MalUrlDto> { new MalUrlDto { Name = "Kids" } }
                    : new List<MalUrlDto>()
            };
        }
    }
}
