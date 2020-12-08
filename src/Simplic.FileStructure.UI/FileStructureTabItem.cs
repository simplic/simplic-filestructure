using Simplic.Framework.DBUI;
using Simplic.Framework.Extension;
using Simplic.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Own Implementation of tab item to get stack name and instance data guid 
    /// </summary>
    public class FileStructureTabItem : TabItem
    {
        #region Fields
        private IFileStructureService fileStructureService;
        private FileStructureControl fileStructureControl;
        private ILocalizationService localizationService;
        private bool loaded;
        private bool instanceDataChanged;
        #endregion

        public FileStructureTabItem()
        {

            this.fileStructureService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureService>();
            this.localizationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILocalizationService>();
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);

            if (instanceDataChanged)
                Initialize();

        }

        private void Initialize()
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                FileStructure fileStructure = null;
                if (instanceDataChanged || Content == null)
                {
                    fileStructureControl = new FileStructureControl();
                    Content = fileStructureControl;
                }
                fileStructure = fileStructureService.GetByInstanceDataGuid(InstanceDataGuid);
                if (fileStructure == null)
                {
                    MessageBoxResult selectFromTemplateResult = MessageBoxResult.None;

                    selectFromTemplateResult = MessageBox.Show(localizationService.Translate("filestructure_select_template_msg"), localizationService.Translate("filestructure_select_template_title"), MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (selectFromTemplateResult == MessageBoxResult.No)
                    {
                        // Create new, maybe from template? 
                        fileStructure = new FileStructure
                        {
                            InstanceDataGuid = InstanceDataGuid,
                            StackGuid = StackHelper.Singleton.GetStackGuidByName(StackName),
                            IsTemplate = false
                        };
                    }
                    else
                    {
                        AsyncItemBox templateItemBox = null;
                        templateItemBox = ItemBoxManager.GetItemBoxFromDB("IB_FileStructureTemplate");
                        templateItemBox.ShowDialog();

                        if (templateItemBox.SelectedItem != null)
                        {
                            var templateId = (Guid)templateItemBox.GetSelectedItemCell("Id");
                            var template = fileStructureService.Get(templateId);

                            // Copy template and connect with instance data entry
                            fileStructure = template.Copy();
                            fileStructure.IsTemplate = false;
                            fileStructure.InstanceDataGuid = InstanceDataGuid;
                            fileStructure.StackGuid = StackHelper.Singleton.GetStackGuidByName(StackName);
                        }
                    }
                }

                if (fileStructure == null)
                    return;

                fileStructureService.Save(fileStructure);
                // Initialize data context and keep load order
                fileStructureControl.Initialize(fileStructure);
                instanceDataChanged = false;
            }));
        }

        #region DependencyProperty
        public static readonly DependencyProperty StackNameProperty =
         DependencyProperty.Register("StackName", typeof(string), typeof(FileStructureTabItem), new
            PropertyMetadata(""));

        public string StackName
        {
            get { return (string)GetValue(StackNameProperty); }
            set { SetValue(StackNameProperty, value); }
        }
        public static readonly DependencyProperty InstanceDataGuidProperty =
         DependencyProperty.Register("InstanceDataGuid", typeof(Guid), typeof(FileStructureTabItem), new
            PropertyMetadata(Guid.Empty, new PropertyChangedCallback(OnInstanceDataGuidChanged)));

        public Guid InstanceDataGuid
        {
            get { return (Guid)GetValue(InstanceDataGuidProperty); }
            set { SetValue(InstanceDataGuidProperty, value); }
        }

        private static void OnInstanceDataGuidChanged(DependencyObject d,
           DependencyPropertyChangedEventArgs e)
        {
            FileStructureTabItem fileStructureTabItem = d as FileStructureTabItem;
            fileStructureTabItem.OnInstanceDataGuidChanged(e);
        }

        private void OnInstanceDataGuidChanged(DependencyPropertyChangedEventArgs e)
        {
            instanceDataChanged = true;
            if (IsSelected)
                Initialize();
        }
        #endregion

    }
}
