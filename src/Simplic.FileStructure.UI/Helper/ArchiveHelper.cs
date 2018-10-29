using Simplic.Base;
using Simplic.DataStack;
using Simplic.Document;
using Simplic.Framework.DynamicUI;
using Simplic.Framework.Extension;
using Simplic.Framework.Extension.UI;
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

            foreach (var file in files)
            {
                // Show document
                var win = DynamicUIManager.Singleton.GetNew("Win_Document");
                win.NewFile(file, stackService.GetStackId("STACK_Document"), 1, 1, 1);

                // win.add_document_path(fileStructure, directory);

                win.Show();
            }
        }
    }
}
