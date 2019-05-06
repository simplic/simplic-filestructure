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
    public class BaseFieldTypeEditor : ApplicationWindow<Guid, FieldType, FieldTypeEditorViewModel, IFieldTypeService>
    {
        /// <summary>
        /// Initialize editor
        /// </summary>
        /// <param name="service">Service instance</param>
        public BaseFieldTypeEditor(IFieldTypeService service) : base(service)
        {

        }
    }

    /// <summary>
    /// Interaction logic for FieldTypeEditor.xaml
    /// </summary>
    public partial class FieldTypeEditor : BaseFieldTypeEditor, IFieldTypeEditor
    {
        /// <summary>
        /// Initialize editor
        /// </summary>
        /// <param name="service">Service instance</param>
        public FieldTypeEditor(IFieldTypeService service) : base(service)
        {
            InitializeComponent();
        }
    }
}
