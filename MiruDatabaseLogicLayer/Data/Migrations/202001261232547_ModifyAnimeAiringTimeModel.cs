// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

namespace MiruDatabaseLogicLayer.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class ModifyAnimeAiringTimeModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnimeAiringTimes", "URL", c => c.String());
            AddColumn("dbo.AnimeAiringTimes", "ImageURL", c => c.String());
            AddColumn("dbo.AnimeAiringTimes", "LocalBroadcastTime", c => c.DateTime());
            AddColumn("dbo.AnimeAiringTimes", "WatchedEpisodes", c => c.Int());
            AddColumn("dbo.AnimeAiringTimes", "TotalEpisodes", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.AnimeAiringTimes", "TotalEpisodes");
            DropColumn("dbo.AnimeAiringTimes", "WatchedEpisodes");
            DropColumn("dbo.AnimeAiringTimes", "LocalBroadcastTime");
            DropColumn("dbo.AnimeAiringTimes", "ImageURL");
            DropColumn("dbo.AnimeAiringTimes", "URL");
        }
    }
}