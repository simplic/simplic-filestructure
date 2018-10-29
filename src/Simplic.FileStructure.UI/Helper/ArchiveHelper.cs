using Simplic.Base;
using Simplic.DataStack;
using Simplic.Document;
using Simplic.Framework.DynamicUI;
using Simplic.Framework.Extension;
using Simplic.Framework.Extension.UI;
using Simplic.Framework.UI;
using Simplic.Icon;
using System;
using System.Windows;

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
                var win = DynamicUIManager.Singleton.GetNew("Win_Document");
                win.NewFile(path, stackService.GetStackId("STACK_Document"), 1, 1, 1);

                // win.add_document_path(fileStructure, directory);

                win.Show();
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
            var documentService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDocumentService>();
            var stackService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IStackService>();
            var fileStructureService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureService>();
            var directoryTypeService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryTypeService>();
            var iconService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IIconService>();

            foreach (var file in files)
            {
                // Show document
                var win = DynamicUIManager.Singleton.GetNew("Win_Document");
                win.NewFile(file, stackService.GetStackId("STACK_Document"), 1, 1, 1);

                var documentWin = win as StackBasedWindow;
                Console.WriteLine($"Window instance: {documentWin.Title}");

                documentWin.Loaded += (s, e) =>
                {
                    var newPath = new FileStructureDocumenPath
                    {
                        DirectoryGuid = directory.Id,
                        FileStructureGuid = fileStructure.Id,
                        DocumentGuid = documentWin.GetInstanceDataGuid()
                    };

                    Console.WriteLine("Path created");

                    var overViewControl = WPFVisualTreeHelper.FindChild<DocumentPathOverview>(documentWin);
                    Console.WriteLine($"Control: {overViewControl?.ToString() ?? "NULL"}");

                    overViewControl.ViewModel.Paths.Add(new DocumentPathViewModel(newPath, fileStructureService, directoryTypeService, iconService, stackService)
                    {
                        Parent = overViewControl.ViewModel
                    });
                };

                win.Show();
            }
        }
    }
}
