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
    public class BaseDirectoryTypeEditor : ApplicationWindow<Guid, DirectoryType, DirectoryTypeEditorViewModel, IDirectoryTypeService>
    {
        /// <summary>
        /// Initialize editor
        /// </summary>
        /// <param name="service">Service instance</param>
        public BaseDirectoryTypeEditor(IDirectoryTypeService service) : base(service)
        {

        }
    }

    /// <summary>
    /// Interaction logic for DirectoryTypeEditor.xaml
    /// </summary>
    public partial class DirectoryTypeEditor : BaseDirectoryTypeEditor, IDirectoryTypeEditor
    {
        private readonly IDirectoryTypeClassificationService directoryTypeClassificationService;

        /// <summary>
        /// Initialize editor
        /// </summary>
        /// <param name="service">Service instance</param>
        public DirectoryTypeEditor(IDirectoryTypeService service) : base(service)
        {
            directoryTypeClassificationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryTypeClassificationService>();
            
            InitializeComponent();
        }

        /// <summary>
        /// Moves an entry from the list of chosen classifications back to the pool of available classifications
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveToPool_Click(object sender, RoutedEventArgs e)
        {
            var fields = LvAvailableFields.SelectedItems;

            foreach(var field in fields)
            {
                var type = (DirectoryClassification) field;

                this.ViewModel.AvailableDirectoryClassifications.Remove(type);
                this.ViewModel.ChosenDirectoryClassifications.Add(type);
            }

            LvAvailableFields.Items.Refresh();
            LDirectoryClassifications.Items.Refresh();
        }


        /// <summary>
        /// Moves a entry from the pool of available classifications to the list of chosen classifications
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteFromPool_Click(object sender, RoutedEventArgs e)
        {
            var fields = LDirectoryClassifications.SelectedItems;

            foreach (var field in fields)
            {
                var type = (DirectoryClassification) field;

                this.ViewModel.ChosenDirectoryClassifications.Remove(type);
                this.ViewModel.AvailableDirectoryClassifications.Add(type);
            }

            LvAvailableFields.Items.Refresh();
            LDirectoryClassifications.Items.Refresh();
        }

        /// <summary>
        /// Saves chosen classifications to database
        /// </summary>
        /// <param name="e"></param>
        public override void OnSave(WindowSaveEventArg e)
        {
            directoryTypeClassificationService.SaveFieldTypes(this.ViewModel.ChosenDirectoryClassifications, this.ViewModel.AvailableDirectoryClassifications, this.ViewModel.Model);
            base.OnSave(e);
        }

        /// <summary>
        /// Deletes all directory type classifications
        /// </summary>
        /// <param name="e"></param>
        public override void OnDelete(WindowDeleteEventArg e)
        {
            directoryTypeClassificationService.DeleteAll(this.ViewModel.Model);
            base.OnDelete(e);
        }
    }
}
