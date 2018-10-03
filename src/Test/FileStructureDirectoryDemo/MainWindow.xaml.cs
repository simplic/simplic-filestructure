using Simplic.FileStructure;
using Simplic.FileStructure.UI;
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

namespace FileStructureDirectoryDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new FileStructureViewModel();

            var structure = new FileStructure();
            structure.Directories.Add(new Directory()
            {
                Name = "Root 1"
            });

            structure.Directories.Add(new Directory() { Name = "Root 2" });

            structure.Directories.Add(new Directory() { Name = "Sub 1 1", Parent = structure.Directories[0] });
            structure.Directories.Add(new Directory() { Name = "Sub 1 2", Parent = structure.Directories[2] });

            viewModel.Initialize(structure);
            
            DataContext = viewModel;
        }
    }
}
