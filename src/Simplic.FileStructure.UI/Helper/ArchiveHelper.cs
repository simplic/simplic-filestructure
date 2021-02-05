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
            var documentService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDocumentService>();
            var stackService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IStackService>();
            var fileStructureService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureService>();
            var directoryTypeService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryTypeService>();
            var iconService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IIconService>();

            // Show document
            var win = DynamicUIManager.Singleton.GetNew("Win_Document");
            win.NewFile(filePath, stackService.GetStackId("STACK_Document"), 1, 1, 1);

            var documentWin = win as StackBasedWindow;
            Console.WriteLine($"Window instance: {documentWin.Title}");

            FileStructureDocumenPath newPath = null;
            documentWin.Loaded += (s, e) =>
            {
                newPath = new FileStructureDocumenPath
                {
                    DirectoryGuid = directory.Id,
                    WorkflowId = directory.WorkflowId,
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

            documentWin.Closed += (s, e) =>
            {
                if (directory.WorkflowId != null)
                {
                    if (documentWin.WindowMode == WindowMode.Edit)
                    {
                        var configurationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<Workflow.IDocumentWorkflowConfigurationService>();
                        var workflow = configurationService.Get(directory.WorkflowId.Value);
                        if (!string.IsNullOrWhiteSpace(workflow.AccessProviderName))
                        {
                            var accessProvider = CommonServiceLocator.ServiceLocator.Current.GetInstance<Workflow.IDocumentWorkflowAccessProvider>(workflow.AccessProviderName);
                            accessProvider.SetUserAccess(GlobalSettings.UserId, documentWin.GetInstanceDataGuid(), newPath.Id, fileStructure.Id, workflow);
                        }
                    }
                }
            };

            win.Show();
        }
    }
}
