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
    /// Base editor
    /// </summary>
    public class BaseDirectoryClassificationEditor : ApplicationWindow<Guid, DirectoryClassification, DirectoryClassificationEditorViewModel, IDirectoryClassificationService>
    {
        /// <summary>
        /// Initialize editor
        /// </summary>
        /// <param name="service">Service instance</param>
        public BaseDirectoryClassificationEditor(IDirectoryClassificationService service) : base(service)
        {

        }
    }

    /// <summary>
    /// Interaction logic for DirectoryClassificationEditor.xaml
    /// </summary>
    public partial class DirectoryClassificationEditor : BaseDirectoryClassificationEditor, IDirectoryClassificationEditor
    {
        private readonly IDirectoryClassificationFieldService directoryClassificationFieldService;
        private readonly IDirectoryClassificationService directoryClassificationService;

        /// <summary>
        /// Initialize editor
        /// </summary>
        /// <param name="service">Service instance</param>
        public DirectoryClassificationEditor(IDirectoryClassificationService service) : base(service)
        {
            directoryClassificationFieldService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryClassificationFieldService>(); ;
            directoryClassificationService = service;

            InitializeComponent();
        }

        /// <summary>
        /// Moves an entry from the list of chosen types back to the pool of available types
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveToPool_Click(object sender, RoutedEventArgs e)
        {
            var fields = LvAvailableFields.SelectedItems;

            foreach(var field in fields)
            {
                var type = (FieldType) field;

                this.ViewModel.AvailableFieldTypes.Remove(type);
                this.ViewModel.ChosenFieldTypes.Add(type);
            }

            LvAvailableFields.Items.Refresh();
            LDirectoryClassifications.Items.Refresh();
        }

        /// <summary>
        /// Moves a entry from the pool of available types to the list of chosen types
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteFromPool_Click(object sender, RoutedEventArgs e)
        {
            var fields = LDirectoryClassifications.SelectedItems;

            foreach (var field in fields)
            {
                var type = (FieldType) field;

                this.ViewModel.ChosenFieldTypes.Remove(type);
                this.ViewModel.AvailableFieldTypes.Add(type);
            }

            LvAvailableFields.Items.Refresh();
            LDirectoryClassifications.Items.Refresh();
        }

        /// <summary>
        /// Saves chosen field types to database
        /// </summary>
        /// <param name="e"></param>
        public override void OnSave(WindowSaveEventArg e)
        {
            directoryClassificationFieldService.SaveFieldTypes(this.ViewModel.ChosenFieldTypes, this.ViewModel.AvailableFieldTypes, this.ViewModel.Model);
            base.OnSave(e);
        }

        /// <summary>
        /// Deletes all directory classification fields
        /// </summary>
        /// <param name="e"></param>
        public override void OnDelete(WindowDeleteEventArg e)
        {
            directoryClassificationFieldService.DeleteAll(this.ViewModel.Model);
            base.OnDelete(e);
        }
    }
}
