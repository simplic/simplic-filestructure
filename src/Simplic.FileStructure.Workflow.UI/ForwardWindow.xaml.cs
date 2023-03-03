using Simplic.Framework.DBUI;
using Simplic.Framework.UI;
using System.Collections.Generic;
using System.Windows;

namespace Simplic.FileStructure.Workflow.UI
{
    /// <summary>
    /// Interaction logic for MultiItemBox.xaml
    /// </summary>
    public partial class ForwardWindow : DefaultRibbonWindow
    {
        /// <summary>
        /// Gets or sets a boolean
        /// It's true when the forward window will be saved, otherwise always false.
        /// </summary>
        public bool IsSave { get; set; }

        /// <summary>
        /// Create multi-itembox.
        /// </summary>
        /// <param name="dictParams">The params for the itembox settings.</param>
        public ForwardWindow(Dictionary<string, string> dictParams)
        {
            InitializeComponent();
            DataContext = new ForwardViewModel(dictParams);
        }

        /// <summary>
        /// Saves the window and set a boolean is save to true.
        /// </summary>
        /// <param name="e"></param>
        public override void OnSave(WindowSaveEventArg e)
        {
            IsSave = true;
            base.OnSave(e);
        }
    }
}