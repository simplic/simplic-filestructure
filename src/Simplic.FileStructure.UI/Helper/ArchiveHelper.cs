using Simplic.Base;
using Simplic.DataStack;
using Simplic.Document;
using Simplic.Document.UI;
using Simplic.Framework.DynamicUI;
using Simplic.Framework.Extension;
using Simplic.Framework.Extension.UI;
using Simplic.Framework.UI;
using Simplic.Icon;
using System;
using System.Windows;
using Prism.Events;
using Simplic.Document.UI.Event;

namespace Simplic.FileStructure.UI.Helper
{
    /// <summary>
    /// Scan helper
    /// </summary>
    public class ArchiveHelper
    {
        /// <summary>
        /// Scan and connect
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        /// <param name="directory">Directory instance</param>
        public static void ArchiveFromScanClient(FileStructure fileStructure, Directory directory)
        {
            var documentService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDocumentService>();
            var stackService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IStackService>();

            DefaultScanWindow scanWindow = new DefaultScanWindow();
            scanWindow.ShowDialog();

            var result = scanWindow.GetScanResult();

            if (result.Save == true && result.MultipageTiff != null)
            {
                // Save as PDF
                string path = GlobalSettings.AppDataPath + "\\Temp\\Blobs\\" + (Guid.NewGuid().ToString()) + ".pdf";
                var pdf = Framework.DocumentProcessing.Document.Convert.TiffToPdf(result.MultipageTiff);
                System.IO.File.WriteAllBytes(path, pdf);

                // Show document
                ArchiveFile(fileStructure, directory, path);
            }
        }

        /// <summary>
        /// Scan and connect
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        /// <param name="directory">Directory instance</param>
        public static void ArchiveFromClipboard(FileStructure fileStructure, Directory directory)
        {
            var files = Clipboard.GetFileDropList();

            foreach (var file in files)
            {
                ArchiveFile(fileStructure, directory, file);
            }
        }

        /// <summary>
        /// Scan and connect
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        /// <param name="directory">Directory instance</param>
        /// <param name="filePath">Path to the file to archvie</param>
        public static void ArchiveFile(FileStructure fileStructure, Directory directory, string filePath)
        {
            var eventAggregator = CommonServiceLocator.ServiceLocator.Current.GetInstance<IEventAggregator>();
            var createDocumentEvent = eventAggregator.GetEvent<Document.UI.Event.CreateDocumentEvent>();

            var args = new CreateDocumentEventArgs
            {
                Path = filePath
            };

            args.DocumentPaths.Add(new CreateDocumentFileStructurePath
            {
                DirectoryId = directory.Id,
                WorkflowId = directory.WorkflowId.GetValueOrDefault(),
                FileStructureGuid = fileStructure.Id
            });

            createDocumentEvent.Publish(args);
        }
    }
}
