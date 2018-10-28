using Simplic.Base;
using Simplic.Framework.Extension;
using Simplic.Framework.Extension.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.UI.Helper
{
    /// <summary>
    /// Scan helper
    /// </summary>
    public class ScanHelper
    {
        /// <summary>
        /// Scan and connect
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        /// <param name="directory">Directory instance</param>
        public static void Scan(FileStructure fileStructure, Directory directory)
        {
            DefaultScanWindow scanWindow = new DefaultScanWindow();
            scanWindow.ShowDialog();

            var result = scanWindow.GetScanResult();

            if (result.Save == true && result.MultipageTiff != null)
            {
                // Save as PDF
                string path = GlobalSettings.AppDataPath + "\\Temp\\Blobs\\" + (Guid.NewGuid().ToString()) + ".pdf";
                var pdf = Simplic.Framework.DocumentProcessing.Document.Convert.TiffToPdf(result.MultipageTiff);
                System.IO.File.WriteAllBytes(path, pdf);

                // Start archvie process
                var stackParameter = new Framework.Extension.DocCenterParameter("STACK_Document", Guid.Parse("12C9B95B-BD33-4FA0-9CA1-05E11122018C"));

                ArchivManager.Singleton.Archive(path, stackParameter);
            }
        }
    }
}
