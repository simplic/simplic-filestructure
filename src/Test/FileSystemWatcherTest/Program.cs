using Simplic.Cache;
using Simplic.Cache.Service;
using Simplic.FileStructure;
using Simplic.FileStructure.Data.DB;
using Simplic.FileStructure.Service;
using Simplic.FileStructure.Sync.FileSystem;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace FileSystemWatcherTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<ISyncStorageWatcherService, SyncStorageWatcherService>();
            unityContainer.RegisterType<ISyncStorageHashService, SyncStorageHashService>();
            unityContainer.RegisterType<ISyncStorageService, SyncStorageService>();
            unityContainer.RegisterType<IFileStructureRepository, FileStructureRepository>();
            unityContainer.RegisterType<IFileStructureService, FileStructureService>();
            unityContainer.RegisterType<ISqlService, DummySql>();
            unityContainer.RegisterType<ISqlColumnService, DummyColumnService>();
            unityContainer.RegisterType<ICacheService, CacheService>();

            Console.WriteLine("Start watcher");
            var watcher = unityContainer.Resolve<ISyncStorageWatcherService>();
            watcher.Initialize(new Simplic.FileStructure.FileStructure
            {
                UseFileSync = true,
                SyncPath = @"C:\Users\beggers.SPIEGELBURG\Sources"
            });
            Console.WriteLine("Watching...");

            while (true)
            {
                watcher.Collect();
                Thread.Sleep(100);
            }

            Console.ReadLine();
        }
    }
}
