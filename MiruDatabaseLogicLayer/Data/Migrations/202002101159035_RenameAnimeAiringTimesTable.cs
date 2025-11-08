// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

namespace MiruDatabaseLogicLayer.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class RenameAnimeAiringTimesTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AnimeAiringTimes", newName: "MiruAiringAnimeModels");
        }

        public override void Down()
        {
            RenameTable(name: "dbo.MiruAiringAnimeModels", newName: "AnimeAiringTimes");
        }
    }
}