// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using Autofac.Extras.Moq;
using MiruLibrary.Models;
using Moq;
using MyInternetConnectionLibrary.Anime;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Miru.Tests.ModelsTests
{
    public class CurrentUserAnimeListModelTests
    {
        [Fact]
        public async Task GetCurrentUserAnimeList_OnHttpRequestException_ReturnsFalseAndExpectedErrorMessage()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                mock.Mock<IAnimeApiClient>()
                    .Setup(x => x.GetUserAnimeListAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new HttpRequestException());
                var expectedErrorMessage = "Problems with internet connection!";
                var sut = mock.Create<CurrentUserAnimeListModel>();

                // Act
                var (result, errorMessage) = await sut.GetCurrentUserAnimeList(It.IsAny<string>());

                // Assert
                Assert.False(result);
                Assert.Equal(expectedErrorMessage, errorMessage);
            }
        }

        [Fact]
        public async Task GetCurrentUserAnimeList_OnAnimeApiNotFoundException_ReturnsFalseAndExpectedErrorMessage()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                mock.Mock<IAnimeApiClient>()
                    .Setup(x => x.GetUserAnimeListAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new AnimeApiNotFoundException("not found"));
                var expectedErrorMessage = $"Could not find the user \"{ It.IsAny<string>() }\". Please make sure you typed in the name correctly.";
                var sut = mock.Create<CurrentUserAnimeListModel>();

                // Act
                var (result, errorMessage) = await sut.GetCurrentUserAnimeList(It.IsAny<string>());

                // Assert
                Assert.False(result);
                Assert.Equal(expectedErrorMessage, errorMessage);
            }
        }

        [Fact]
        public async Task GetCurrentUserAnimeList_OnNoExceptions_ReturnsTrueAndEmptyErrorMessage()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                mock.Mock<IAnimeApiClient>()
                    .Setup(x => x.GetUserAnimeListAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new UserAnimeListPageEnvelope { Data = new List<UserAnimeListEntryDto>() });
                var expectedErrorMessage = string.Empty;
                var sut = mock.Create<CurrentUserAnimeListModel>();

                // Act
                var (result, errorMessage) = await sut.GetCurrentUserAnimeList(It.IsAny<string>());

                // Assert
                Assert.True(result);
                Assert.Equal(expectedErrorMessage, errorMessage);
            }
        }

        [Fact]
        public async Task GetCurrentUserAnimeList_FiltersEntriesByWatchingStatus()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                mock.Mock<IAnimeApiClient>()
                    .Setup(x => x.GetUserAnimeListAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new UserAnimeListPageEnvelope
                    {
                        Data = new List<UserAnimeListEntryDto>
                        {
                            CreateEntry(1, 1, 5),      // watching
                            CreateEntry(2, 2, 12),     // completed
                            CreateEntry(3, 4, 3),      // dropped
                            CreateEntry(4, 1, 7),      // watching
                        }
                    });
                var sut = mock.Create<CurrentUserAnimeListModel>();

                // Act
                var (result, errorMessage) = await sut.GetCurrentUserAnimeList(It.IsAny<string>());

                // Assert
                Assert.True(result);
                Assert.Equal(2, sut.UserAnimeListData.Anime.Count);
                Assert.Equal(1, sut.UserAnimeListData.Anime[0].MalId);
                Assert.Equal(4, sut.UserAnimeListData.Anime[1].MalId);
                Assert.Equal(5, sut.UserAnimeListData.Anime[0].WatchedEpisodes);
                Assert.Equal(12, sut.UserAnimeListData.Anime[0].TotalEpisodes);
            }
        }

        [Theory]
        [InlineData(50)]
        [InlineData(299)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetCurrentUserDroppedAnimeList_LessThan300DroppedAnimes_ReturnTrueAndCorrectCount(int expectedDroppedAnimeCount)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                var testData = new UserAnimeListPageEnvelope
                {
                    Data = CreateEntries(UserAnimeListStatus.Dropped, expectedDroppedAnimeCount)
                };
                mock.Mock<IAnimeApiClient>()
                    .Setup(x => x.GetUserAnimeListAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(testData);
                var sut = mock.Create<CurrentUserAnimeListModel>();

                // Act
                var (result, errorMessage) = await sut.GetCurrentUserDroppedAnimeList(It.IsAny<string>());

                // Assert
                Assert.True(result);
                Assert.Equal(string.Empty, errorMessage);
                Assert.Equal(expectedDroppedAnimeCount, sut.UserDroppedAnimeListData.Anime.Count);
            }
        }

        [Fact]
        public async Task GetCurrentUserDroppedAnimeList_Exception_ReturnFalseAndErrorMessage()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                mock.Mock<IAnimeApiClient>()
                    .Setup(x => x.GetUserAnimeListAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new Exception());
                var sut = mock.Create<CurrentUserAnimeListModel>();

                // Act
                var (result, errorMessage) = await sut.GetCurrentUserDroppedAnimeList(It.IsAny<string>());

                // Assert
                Assert.False(result);
                Assert.Equal("Problem with getting user's dropped anime list!", errorMessage);
            }
        }

        [Theory]
        [InlineData(300)]
        [InlineData(600)]
        [InlineData(2137)]
        [InlineData(3939)]
        public async Task GetCurrentUserDroppedAnimeList_300OrMoreDroppedAnimes_ReturnTrueAndCorrectCount(int expectedDroppedAnimeCount)
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                const int pageSize = 300;
                int entriesLeft = expectedDroppedAnimeCount;

                mock.Mock<IAnimeApiClient>()
                    .Setup(x => x.GetUserAnimeListAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() =>
                    {
                        var entriesOnPage = Math.Min(pageSize, entriesLeft);
                        entriesLeft -= entriesOnPage;

                        return new UserAnimeListPageEnvelope
                        {
                            Data = CreateEntries(UserAnimeListStatus.Dropped, entriesOnPage),
                        };
                    });
                var sut = mock.Create<CurrentUserAnimeListModel>();

                // Act
                var (result, errorMessage) = await sut.GetCurrentUserDroppedAnimeList(It.IsAny<string>());

                // Assert
                Assert.True(result);
                Assert.Equal(string.Empty, errorMessage);
                Assert.Equal(expectedDroppedAnimeCount, sut.UserDroppedAnimeListData.Anime.Count);
            }
        }

        private static UserAnimeListEntryDto CreateEntry(long malId, int status, int watchedEpisodes)
        {
            return new UserAnimeListEntryDto
            {
                WatchingStatus = status,
                EpisodesWatched = watchedEpisodes,
                Anime = new AnimeDto { MalId = malId, Episodes = 12 }
            };
        }

        private static List<UserAnimeListEntryDto> CreateEntries(int status, int count)
        {
            var entries = new List<UserAnimeListEntryDto>();
            for (int i = 0; i < count; i++)
            {
                entries.Add(CreateEntry(i + 1, status, 0));
            }
            return entries;
        }
    }
}
