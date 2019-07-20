using Simplic.Base;
using Simplic.DataStack;
using Simplic.Framework.DBUI;
using Simplic.Framework.DocumentProcessing.Outlook;
using Simplic.Localization;
using Simplic.UI.GridView;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.DragDrop;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Interaction logic for DirectoryFieldControl.xaml
    /// </summary>
    public partial class DirectoryFieldControl : UserControl
    {
        private static ILocalizationService localizationService;

        /// <summary>
        /// Initialize control
        /// </summary>
        public DirectoryFieldControl()
        {
            InitializeComponent();

            if (localizationService == null)
                localizationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
        }

        /// <summary>
        /// Initialize control and fill data
        /// </summary>
        /// <param name="directory">directory instance</param>
        /// <param name="fileStructure">File structure instance</param>
        public DirectoryFieldViewModel Initialize(Directory directory, FileStructure fileStructure, Boolean ExpanderMode = false)
        {
            var viewModel = new DirectoryFieldViewModel();
            viewModel.Initialize(directory, fileStructure);

            if (ExpanderMode)
                viewModel.EnableExpanderMode();

            DataContext = viewModel;

            return viewModel;
        }

        

        /// <summary>
        /// Gets the current datacontext as <see cref="DirectoryFieldViewModel"/>
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
