// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using Autofac.Extras.Moq;
using Moq;
using MyInternetConnectionLibrary;
using MyInternetConnectionLibrary.Anime;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Miru.Tests.MyInternetConnectionLibraryTests
{
    public class WebServiceTests
    {
        [Fact]
        public async Task TryToGetAnimeInfo_GivenValidMalId_ReturnsAnime()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Func<IWebClientWrapper> mockWebClientFunc = () => { return Mock.Of<IWebClientWrapper>(); };
                mock.Mock<IWebService>().Setup(x => x.CreateWebClient).Returns(mockWebClientFunc);
                var testData = new AnimeDto();
                mock.Mock<IAnimeApiClient>()
                    .Setup(x => x.GetAnimeFullAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(testData);
                var sut = mock.Create<WebService>();

                var result = await sut.TryToGetAnimeInfo(default, default, mock.Mock<IAnimeApiClient>().Object);

                Assert.Equal(testData, result);
            }
        }

        [Fact]
        public async Task TryToGetAnimeInfo_GivenAnimeApiException_RetryAndReturnAnime()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Func<IWebClientWrapper> mockWebClientFunc = () => { return Mock.Of<IWebClientWrapper>(); };
                mock.Mock<IWebService>()
                    .Setup(x => x.CreateWebClient)
                    .Returns(mockWebClientFunc);
                var testData = new AnimeDto();
                mock.Mock<IAnimeApiClient>()
                    .SetupSequence(x => x.GetAnimeFullAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new AnimeApiException("error"))
                    .ReturnsAsync(testData);
                var sut = mock.Create<WebService>();

                var result = await sut.TryToGetAnimeInfo(default, default, mock.Mock<IAnimeApiClient>().Object);

                mock.Mock<IAnimeApiClient>()
                    .Verify(x => x.GetAnimeFullAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
                Assert.Equal(testData, result);
            }
        }

        [Fact]
        public async Task TryToGetAnimeInfo_GivenHttpRequestException_ThrowsNoInternetConnectionException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Func<IWebClientWrapper> mockWebClientFunc = () => { return Mock.Of<IWebClientWrapper>(); };
                mock.Mock<IWebService>()
                    .Setup(x => x.CreateWebClient)
                    .Returns(mockWebClientFunc);
                mock.Mock<IAnimeApiClient>()
                    .Setup(x => x.GetAnimeFullAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new HttpRequestException());
                var sut = mock.Create<WebService>();

                var result = await Assert.ThrowsAsync<NoInternetConnectionException>(
                    () => sut.TryToGetAnimeInfo(default, default, mock.Mock<IAnimeApiClient>().Object)
                    );
                Assert.Equal("No internet connection", result.Message);
            }
        }
    }
}
