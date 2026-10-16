// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using MiruLibrary.Models;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MiruDatabaseLogicLayer
{
    // app's default db context
    [ExcludeFromCodeCoverage]
    public class MiruDbContext : DbContext, IMiruDbContext
    {
        public MiruDbContext(DbContextOptions<MiruDbContext> options) : base(options)
        {
        }

        public int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlRaw(sql, parameters);
        }

        public bool EnsureCreated() => Database.EnsureCreated();

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        // set db tables
        public virtual DbSet<SyncedMyAnimeListUser> SyncedMyAnimeListUsers { get; set; }
        public virtual DbSet<MiruAnimeModel> MiruAnimeModels { get; set; }

    }
}