// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

namespace MiruDatabaseLogicLayer.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class AddAnimeAiringTimesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnimeAiringTimes",
                c => new
                {
                    MalId = c.Long(nullable: false),
                    Broadcast = c.String(),
                    Title = c.String(),
                })
                .PrimaryKey(t => t.MalId);
        }

        public override void Down()
        {
            DropTable("dbo.AnimeAiringTimes");
        }
    }
}