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
        /// <summary>
        /// Initialize editor
        /// </summary>
        /// <param name="service">Service instance</param>
        public DirectoryTypeEditor(IDirectoryTypeService service) : base(service)
        {
            InitializeComponent();
        }
    }
}
