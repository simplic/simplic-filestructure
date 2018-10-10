using Simplic.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Interaction logic for FileStructureWindow.xaml
    /// </summary>
    public partial class FileStructureWindow : DefaultRibbonWindow
    {
        /// <summary>
        /// Initialize file structure window
        /// </summary>
        public FileStructureWindow()
        {
            InitializeComponent();

            AllowPaging = false;
        }

        /// <summary>
        /// Initialize file structure window
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        public void Initialize(FileStructure fileStructure)
        {
            var viewModel = new FileStructureViewModel(fileStructureControl.DirectoryTreeView);

            viewModel.Initialize(fileStructure);

            DataContext = viewModel;
        }

        /// <summary>
        /// Show sample window
        /// </summary>
        public static void ShowSample()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                var d = new FileStructureWindow();
                d.Initialize(new FileStructure());
                d.Show();
            }));
        }
    }
}
