// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

namespace MiruDatabaseLogicLayer.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class AddAnimeType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MiruAiringAnimeModels", "Type", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.MiruAiringAnimeModels", "Type");
        }
    }
}