﻿using Simplic.DataStack;
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
    /// Interaction logic for FileStructureWindow.xaml
    /// </summary>
    public partial class FileStructureWindow : DefaultRibbonWindow
    {
        private readonly IFileStructureService fileStructureService;
        private readonly IStackService stackService;
        private readonly IRenderingService reportService;

        /// <summary>
        /// Initialize file structure window
        /// </summary>
        public FileStructureWindow()
        {
            InitializeComponent();

            fileStructureService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureService>();
            stackService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IStackService>();
            reportService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IRenderingService>();

            var showReportButton = new RibbonButton
            {
                LargeIconName = "filestructure_report_32x",
                SmallIconName = "filestructure_report_32x",
                TextLocalizationKey = "filestructure_show_report",
                TooltipLocalizationKey = "filestructure_show_report_tooltip",
                Size = Telerik.Windows.Controls.RibbonView.ButtonSize.Large
            };

            showReportButton.Click += (s, e) =>
            {
                var html = reportService.Render(ViewModel.GetStructure());

                var now = DateTime.Now;
                var htmlFilePath = $"{Base.GlobalSettings.AppDataPath}\\Temp\\FileStructure_{now.Year}{now.Month}{now.Day}{now.Hour}{now.Minute}{now.Second}.html";

                System.IO.File.WriteAllText(htmlFilePath, html);
                System.Diagnostics.Process.Start(htmlFilePath);
            };

            RadRibbonDataGroup.Items.Add(showReportButton);

            AllowPaging = false;
        }

        /// <summary>
        /// Tree view mouse double clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTreeViewMouseDoubleClickHandler(object sender, MouseButtonEventArgs args)
        {
            SelectedDirectory = fileStructureControl.ViewModel.SelectedDirectory?.Model;
            if (SelectedDirectory != null && IsInSelectMode)
                this.Close();
        }

        /// <summary>
        /// Initialize file structure window
        /// </summary>
        /// <param name="fileStructure">File structure instance</param>
        public void Initialize(FileStructure fileStructure)
        {
            if (fileStructure.StackGuid != null && fileStructure.InstanceDataGuid != null)
            {
                Title = $"{Title} - {stackService.GetInstanceDataContent(fileStructure.StackGuid.Value, fileStructure.InstanceDataGuid.Value)}";
            }

            DataContext = fileStructureControl.Initialize(fileStructure);

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
                ViewModel.Save();

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

        /// <summary>
        /// Gets or sets whether the window in in select mode
        /// </summary>
        public bool IsInSelectMode
        {
            get;
            set;
        } = false;

        /// <summary>
        /// Gets or sets the selected directory
        /// </summary>
        public Directory SelectedDirectory
        {
            get;
            set;
        }
    }
}
