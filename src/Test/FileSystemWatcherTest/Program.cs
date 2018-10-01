using Simplic.FileStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileSystemWatcherTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start watcher");
            var watcher = new SyncStorageWatcherService(null, null);
            watcher.Initialize(new Simplic.FileStructure.FileStructure
            {
                UseFileSync = true,
                SyncPath = "C:\\support"
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
