// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using Autofac;
using Caliburn.Micro;
using MiruDatabaseLogicLayer;
using MiruLibrary.Models;
using Miru.ViewModels;
using System.Windows;
using MyInternetConnectionLibrary.Anime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using MiruLibrary.Settings;
using MiruLibrary;
using System.IO.Abstractions;
using MyInternetConnectionLibrary;
using MiruLibrary.Services;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Miru
{
    [ExcludeFromCodeCoverage]
    public class Bootstrapper : BootstrapperBase
    {
        private IContainer Container { get; set; }

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();
            ConfigureContainer(builder);
            Container = builder.Build();
        }

        protected void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
            builder.RegisterType<SortedAnimeListsViewModel>().As<ISortedAnimeListsViewModel>();
            builder.RegisterType<ShellViewModel>()
                .As<IShellViewModel>()
                .SingleInstance()
                // example of method dependency injection with autofac
                .OnActivated(e => e.Instance.SetSettingsWriter(e.Context.Resolve<ISettingsWriter>()))
                // example of property dependency injection with autofac
                // there are other ways for it like PropertiesAutowired() or required properties
                // but this is the most explicit way for a specific property
                .WithProperty("ToastNotifierWrapper", new ToastNotifierWrapper());
            builder.RegisterType<MiruDbService>().As<IMiruDbService>();
            builder.RegisterType<SimpleContentDialog>().As<ISimpleContentDialog>();
            builder.RegisterType<CurrentSeasonModel>().As<ICurrentSeasonModel>();
            builder.RegisterType<CurrentUserAnimeListModel>().As<ICurrentUserAnimeListModel>();
            // the url of the anime api (Jikan REST API v4) is taken from the user settings
            builder.Register(ctx => new AnimeApiClient(new HttpClient(), ctx.Resolve<UserSettings>().AnimeApiBaseUrl))
                .As<IAnimeApiClient>()
                .SingleInstance();
            builder.RegisterType<ToastNotifierWrapper>().As<IToastNotifierWrapper>();
            builder.RegisterType<MiruAnimeModelProcessor>().As<IMiruAnimeModelProcessor>();
            var databasePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Miru",
                "MiruDatabase.sqlite");
            builder.Register(_ => new MiruDbContext(
                new DbContextOptionsBuilder<MiruDbContext>()
                    .UseSqlite($"Data Source={databasePath}")
                    .Options))
                .As<IMiruDbContext>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
            builder.RegisterType<FileSystemService>().As<IFileSystemService>();
            builder.RegisterType<SyncedMyAnimeListUser>().As<ISyncedMyAnimeListUser>();
            builder.RegisterType<WebClientWrapper>().As<IWebClientWrapper>();
            builder.RegisterType<WebService>().As<IWebService>();
            builder.RegisterType<MiruAnimeModel>();
            builder.RegisterType<TimerService>().As<ITimerService>();
            builder.RegisterType<UserDataService>().As<IUserDataService>();
            builder.RegisterType<SystemService>().As<ISystemService>();
            // path to the config file is passed here
            builder.RegisterModule(new SettingsModule());
        }

        protected override object GetInstance(Type service, string key)
        {
            return key == null ? Container.Resolve(service) : Container.ResolveKeyed(key, service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            var listOfServiceType = typeof(IList<>).MakeGenericType(service);
            return (IEnumerable<object>)Container.Resolve(listOfServiceType);
        }

        protected override void BuildUp(object instance)
        {
            Container.InjectProperties(instance);
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            // store the local sqlite database in %LOCALAPPDATA%\Miru so the app works
            // without SQL Server / LocalDB installed
            var appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Miru");
            Directory.CreateDirectory(appDataFolder);
            Directory.CreateDirectory(Path.Combine(appDataFolder, "MiruCache"));
            // set starting view for this app
            await DisplayRootViewForAsync<IShellViewModel>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            Container.Resolve<IShellViewModel>().SaveSettings(false);
            base.OnExit(sender, e);
        }
    }
}
