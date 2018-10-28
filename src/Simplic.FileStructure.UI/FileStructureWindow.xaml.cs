using Simplic.Framework.UI;
using Simplic.Log;
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
        private readonly IFileStructureService fileStructureService;

        /// <summary>
        /// Initialize file structure window
        /// </summary>
        public FileStructureWindow()
        {
            InitializeComponent();

            fileStructureService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureService>();
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

            AddPagingObject(fileStructure);
            WindowMode = WindowMode.Edit;
        }

        /// <summary>
        /// Save changes
        /// </summary>
        /// <param name="e"></param>
        public override void OnSave(WindowSaveEventArg e)
        {
            try
            {
                fileStructureService.Save(ViewModel.GetStructure());
                e.IsSaved = true;
            }
            catch (Exception ex)
            {
                LogManagerInstance.Instance.Error("Could not save file structure", ex);
            }
            base.OnSave(e);
        }

        /// <summary>
        /// Gets the current data context as <see cref="FileStructureViewModel"/>
        /// </summary>
        public FileStructureViewModel ViewModel
        {
            get
            {
                return DataContext as FileStructureViewModel;
            }
        }
    }
}
