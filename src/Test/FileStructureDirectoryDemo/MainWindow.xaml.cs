using Newtonsoft.Json;
using Simplic.FileStructure;
using Simplic.FileStructure.UI;
using System;
using System.Collections.Generic;
using System.IO;
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
        private JsonSerializerSettings jsonSettings;
        public MainWindow()
        {
            jsonSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.Auto
            };

            InitializeComponent();

            var viewModel = new FileStructureViewModel();

            var structure = new FileStructure();

            if (File.Exists("Sample.json"))
            {
                structure = JsonConvert.DeserializeObject<FileStructure>(File.ReadAllText("Sample.json"), jsonSettings);
            }

            viewModel.Initialize(structure);
            
            DataContext = viewModel;

            this.Closed += (s, e) => 
            {
                File.WriteAllText("Sample.json", JsonConvert.SerializeObject(viewModel.GetStructure(), jsonSettings));
            };
        }
    }
}
