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
        public FileStructureWindow()
        {
            InitializeComponent();

            var viewModel = new FileStructureViewModel();
            var structure = new FileStructure();

            viewModel.Initialize(structure);

            DataContext = viewModel;
        }

        public static void ShowSame()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                var d = new FileStructureWindow();
                d.Show();
            }));
        }
    }
}
