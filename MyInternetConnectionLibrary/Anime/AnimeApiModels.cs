// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyInternetConnectionLibrary.Anime
{
    /// <summary>
    /// Models of the Jikan REST API v4 responses (the schema is shared with the Tenrai API v1).
    /// </summary>
    public class AnimeDto
    {
        [JsonPropertyName("mal_id")] public long? MalId { get; set; }
        [JsonPropertyName("url")] public string Url { get; set; }
        [JsonPropertyName("images")] public AnimeImagesDto Images { get; set; }
        [JsonPropertyName("trailer")] public AnimeTrailerDto Trailer { get; set; }
        [JsonPropertyName("approved")] public bool? Approved { get; set; }
        [JsonPropertyName("titles")] public List<AnimeTitleDto> Titles { get; set; }
        [JsonPropertyName("title")] public string Title { get; set; }
        [JsonPropertyName("title_english")] public string TitleEnglish { get; set; }
        [JsonPropertyName("title_japanese")] public string TitleJapanese { get; set; }
        [JsonPropertyName("title_synonyms")] public List<string> TitleSynonyms { get; set; }
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("source")] public string Source { get; set; }
        [JsonPropertyName("episodes")] public int? Episodes { get; set; }
        [JsonPropertyName("status")] public string Status { get; set; }
        [JsonPropertyName("airing")] public bool? Airing { get; set; }
        [JsonPropertyName("aired")] public TimePeriodDto Aired { get; set; }
        [JsonPropertyName("duration")] public string Duration { get; set; }
        [JsonPropertyName("rating")] public string Rating { get; set; }
        [JsonPropertyName("score")] public double? Score { get; set; }
        [JsonPropertyName("scored_by")] public int? ScoredBy { get; set; }
        [JsonPropertyName("rank")] public int? Rank { get; set; }
        [JsonPropertyName("popularity")] public int? Popularity { get; set; }
        [JsonPropertyName("members")] public int? Members { get; set; }
        [JsonPropertyName("favorites")] public int? Favorites { get; set; }
        [JsonPropertyName("synopsis")] public string Synopsis { get; set; }
        [JsonPropertyName("background")] public string Background { get; set; }
        [JsonPropertyName("season")] public string Season { get; set; }
        [JsonPropertyName("year")] public int? Year { get; set; }
        [JsonPropertyName("broadcast")] public BroadcastDto Broadcast { get; set; }
        [JsonPropertyName("producers")] public List<MalUrlDto> Producers { get; set; }
        [JsonPropertyName("licensors")] public List<MalUrlDto> Licensors { get; set; }
        [JsonPropertyName("studios")] public List<MalUrlDto> Studios { get; set; }
        [JsonPropertyName("genres")] public List<MalUrlDto> Genres { get; set; }
        [JsonPropertyName("explicit_genres")] public List<MalUrlDto> ExplicitGenres { get; set; }
        [JsonPropertyName("themes")] public List<MalUrlDto> Themes { get; set; }
        [JsonPropertyName("demographics")] public List<MalUrlDto> Demographics { get; set; }
        [JsonPropertyName("relations")] public List<AnimeRelationDto> Relations { get; set; }

        /// <summary>Opening/ending theme texts, only returned by the /anime/{id}/full endpoint.</summary>
        [JsonPropertyName("theme")] public AnimeThemeDto Theme { get; set; }

        [JsonPropertyName("external")] public List<ExternalLinkDto> External { get; set; }
        [JsonPropertyName("streaming")] public List<ExternalLinkDto> Streaming { get; set; }

        // fields returned only by the user anime list endpoint
        [JsonPropertyName("watched_episodes")] public int? WatchedEpisodes { get; set; }
        [JsonPropertyName("total_episodes")] public int? TotalEpisodes { get; set; }
        [JsonPropertyName("airing_status")] public string AiringStatus { get; set; }
        [JsonPropertyName("comments")] public string Comments { get; set; }
    }

    public class AnimeImagesDto
    {
        [JsonPropertyName("jpg")] public ImageDto Jpg { get; set; }
        [JsonPropertyName("webp")] public ImageDto WebP { get; set; }
    }

    public class ImageDto
    {
        [JsonPropertyName("image_url")] public string ImageUrl { get; set; }
        [JsonPropertyName("small_image_url")] public string SmallImageUrl { get; set; }
        [JsonPropertyName("medium_image_url")] public string MediumImageUrl { get; set; }
        [JsonPropertyName("large_image_url")] public string LargeImageUrl { get; set; }
    }

    public class AnimeTrailerDto
    {
        [JsonPropertyName("youtube_id")] public string YoutubeId { get; set; }
        [JsonPropertyName("url")] public string Url { get; set; }
        [JsonPropertyName("embed_url")] public string EmbedUrl { get; set; }
        [JsonPropertyName("images")] public AnimeImagesDto Images { get; set; }
    }

    public class AnimeTitleDto
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("title")] public string Title { get; set; }
    }

    public class MalUrlDto
    {
        [JsonPropertyName("mal_id")] public long? MalId { get; set; }
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("url")] public string Url { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
    }

    public class TimePeriodDto
    {
        [JsonPropertyName("from")] public System.DateTimeOffset? From { get; set; }
        [JsonPropertyName("to")] public System.DateTimeOffset? To { get; set; }
        [JsonPropertyName("prop")] public TimePeriodPropDto Prop { get; set; }
        [JsonPropertyName("string")] public string String { get; set; }
    }

    /// <summary>Parts of a date returned by the api (e.g. the "prop" of the aired period).</summary>
    public class TimePeriodPropDto
    {
        [JsonPropertyName("from")] public DatePartsDto From { get; set; }
        [JsonPropertyName("to")] public DatePartsDto To { get; set; }
    }

    public class DatePartsDto
    {
        [JsonPropertyName("day")] public int? Day { get; set; }
        [JsonPropertyName("month")] public int? Month { get; set; }
        [JsonPropertyName("year")] public int? Year { get; set; }
    }

    public class BroadcastDto
    {
        [JsonPropertyName("day")] public string Day { get; set; }
        [JsonPropertyName("time")] public string Time { get; set; }
        [JsonPropertyName("timezone")] public string Timezone { get; set; }

        /// <summary>Broadcast string ready to use, e.g. "Fridays at 23:00 (JST)".</summary>
        [JsonPropertyName("string")] public string String { get; set; }
    }

    public class AnimeThemeDto
    {
        [JsonPropertyName("openings")] public List<string> Openings { get; set; }
        [JsonPropertyName("endings")] public List<string> Endings { get; set; }
    }

    public class AnimeRelationDto
    {
        [JsonPropertyName("relation")] public string Relation { get; set; }
        [JsonPropertyName("entry")] public List<MalUrlDto> Entry { get; set; }
    }

    public class ExternalLinkDto
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("url")] public string Url { get; set; }
    }

    /// <summary>Envelope of a single item response, e.g. /anime/{id}.</summary>
    public class AnimeDataEnvelope
    {
        [JsonPropertyName("data")] public AnimeDto Data { get; set; }
    }

    /// <summary>Envelope of a paginated response, e.g. /seasons/now or /users/{username}/animelist.</summary>
    public class AnimePageEnvelope
    {
        [JsonPropertyName("data")] public List<AnimeDto> Data { get; set; }
        [JsonPropertyName("pagination")] public PaginationDto Pagination { get; set; }
    }

    /// <summary>
    /// Entry of a user's anime list. The api returns the list status and the progress
    /// of the user next to the anime itself.
    /// </summary>
    public class UserAnimeListEntryDto
    {
        [JsonPropertyName("watching_status")] public int? WatchingStatus { get; set; }
        [JsonPropertyName("score")] public int? Score { get; set; }
        [JsonPropertyName("episodes_watched")] public int? EpisodesWatched { get; set; }
        [JsonPropertyName("is_rewatching")] public bool? IsRewatching { get; set; }
        [JsonPropertyName("priority")] public string Priority { get; set; }
        [JsonPropertyName("anime")] public AnimeDto Anime { get; set; }
    }

    /// <summary>Envelope of the paginated user anime list response.</summary>
    public class UserAnimeListPageEnvelope
    {
        [JsonPropertyName("data")] public List<UserAnimeListEntryDto> Data { get; set; }
        [JsonPropertyName("pagination")] public PaginationDto Pagination { get; set; }
    }

    /// <summary>Status of an entry on a user's anime list (the ids used by MyAnimeList).</summary>
    public static class UserAnimeListStatus
    {
        public const int Watching = 1;
        public const int Completed = 2;
        public const int OnHold = 3;
        public const int Dropped = 4;
        public const int PlanToWatch = 6;
    }

    public class PaginationDto
    {
        [JsonPropertyName("last_visible_page")] public int LastVisiblePage { get; set; }
        [JsonPropertyName("has_next_page")] public bool HasNextPage { get; set; }
        [JsonPropertyName("current_page")] public int? CurrentPage { get; set; }
        [JsonPropertyName("items")] public PaginationItemsDto Items { get; set; }
    }

    public class PaginationItemsDto
    {
        [JsonPropertyName("count")] public int Count { get; set; }
        [JsonPropertyName("total")] public int Total { get; set; }
        [JsonPropertyName("per_page")] public int PerPage { get; set; }
    }
}
