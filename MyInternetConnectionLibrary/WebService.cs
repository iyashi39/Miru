// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using MyInternetConnectionLibrary.Anime;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyInternetConnectionLibrary
{
    public class WebService : IWebService
    {
        // initializing a read-only instance of HttpClient to give the app access to HTTP GET requests
        public HttpClient Client { get; } = new HttpClient();

        public WebService(Func<IWebClientWrapper> createWebClient)
        {
            CreateWebClient = createWebClient;
        }

        public Func<IWebClientWrapper> CreateWebClient { get; private set; }

        // tries to get the detailed anime information about anime with the given mal id, retries after given delay until the internet connection is working
        public async Task<AnimeDto> TryToGetAnimeInfo(long malId, int millisecondsDelay, IAnimeApiClient animeApiClient)
        {
            AnimeDto output = null;

            // if there is no response from API wait for the given time and retry
            while (output == null)
            {
                try
                {
                    // get detailed anime info from the api (the full endpoint contains the opening/ending themes)
                    output = await animeApiClient.GetAnimeFullAsync(malId);
                }
                catch (HttpRequestException)
                {
                    throw new NoInternetConnectionException("No internet connection");
                }
                catch (AnimeApiNotFoundException)
                {
                    // the anime does not exist anymore, there is no point in retrying
                    throw;
                }
                catch (AnimeApiException)
                {
                    await Task.Delay(millisecondsDelay);
                }
                finally
                {
                    await Task.Delay(millisecondsDelay);
                }
            }
            return output;
        }
    }
}
