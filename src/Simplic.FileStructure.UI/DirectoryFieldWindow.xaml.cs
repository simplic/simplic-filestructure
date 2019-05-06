using Simplic.DataStack;
using Simplic.Framework.UI;
using Simplic.Log;
using Simplic.Studio.UI.Navigation;
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
    /// Interaction logic for DirectoryFieldWindow.xaml
    /// </summary>
    public partial class DirectoryFieldWindow : DefaultRibbonWindow
    {
        /// <summary>
        /// Initialize file structure window
        /// </summary>
        public DirectoryFieldWindow()
        {
            InitializeComponent();

            AllowPaging = false;
        }

        /// <summary>
        /// Initialize directory field window
        /// </summary>
        /// <param name="directory">directory instance</param>
        /// <param name="fileStructure">File structure instance</param>
        public void Initialize(Directory directory, FileStructure fileStructure)
        {
            DataContext = directoryFieldControl.Initialize(directory, fileStructure);
            

            //AddPagingObject(directory);
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
                ViewModel.Save();

                e.IsSaved = true;
            }
            catch (Exception ex)
            {
                LogManagerInstance.Instance.Error("Could not save directory metadata", ex);
            }
            base.OnSave(e);
        }

        /// <summary>
        /// Gets the current data context as <see cref="FileStructureViewModel"/>
        /// </summary>
        public DirectoryFieldViewModel ViewModel
        {
            get
            {
                return DataContext as DirectoryFieldViewModel;
            }
        }
    }
}
