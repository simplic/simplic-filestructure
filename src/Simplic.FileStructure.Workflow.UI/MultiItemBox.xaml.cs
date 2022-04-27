using Simplic.Framework.DBUI;
using Simplic.Framework.UI;
using System.Collections.Generic;
using System.Windows;

namespace Simplic.FileStructure.Workflow.UI
{
    /// <summary>
    /// Interaction logic for MultiItemBox.xaml
    /// </summary>
    public partial class MultiItemBox : DefaultRibbonWindow
    {
        /// <summary>
        /// Create multi-itembox.
        /// </summary>
        /// <param name="dictParams">The params for the itembox settings.</param>
        public MultiItemBox(Dictionary<string, string> dictParams)
        {
            InitializeComponent();
            DataContext = new MultiItemBoxViewModel(dictParams);
        }
    }
}