// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

namespace MiruDatabaseLogicLayer.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class AddSyncedUsersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SyncedMyAnimeListUsers",
                c => new
                {
                    Username = c.String(nullable: false, maxLength: 16),
                    SyncTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Username);
        }

        public override void Down()
        {
            DropTable("dbo.SyncedMyAnimeListUsers");
        }
    }
}