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
    /// Interaction logic for FileStructureWindow.xaml
    /// </summary>
    public partial class DirectoryTypeEditor : BaseDirectoryTypeEditor, IDirectoryTypeEditor
    {
        private readonly IDirectoryTypeFieldService directoryTypeFieldService;

        /// <summary>
        /// Initialize editor
        /// </summary>
        /// <param name="service">Service instance</param>
        public DirectoryTypeEditor(IDirectoryTypeService service) : base(service)
        {
            directoryTypeFieldService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryTypeFieldService>();
            
            InitializeComponent();
        }

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
            LvDirectoryTypeFields.Items.Refresh();
        }

        private void BtnDeleteFromPool_Click(object sender, RoutedEventArgs e)
        {
            var fields = LvDirectoryTypeFields.SelectedItems;

            foreach (var field in fields)
            {
                var type = (FieldType) field;

                this.ViewModel.ChosenFieldTypes.Remove(type);
                this.ViewModel.AvailableFieldTypes.Add(type);
            }

            LvAvailableFields.Items.Refresh();
            LvDirectoryTypeFields.Items.Refresh();
        }

        public override void OnSave(WindowSaveEventArg e)
        {
            directoryTypeFieldService.SaveFieldTypes(this.ViewModel.ChosenFieldTypes, this.ViewModel.AvailableFieldTypes, this.ViewModel.Model);
            base.OnSave(e);
        }

        public override void OnDelete(WindowDeleteEventArg e)
        {
            directoryTypeFieldService.DeleteAll(this.ViewModel.Model);
            base.OnDelete(e);
        }
    }
}
