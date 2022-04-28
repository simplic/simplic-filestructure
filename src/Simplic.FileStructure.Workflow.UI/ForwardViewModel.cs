using Simplic.UI.Control;
using Simplic.UI.MVC;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Simplic.FileStructure.Workflow.UI
{
    /// <summary>
    /// Represents a viewmodel for managing multi-itembox-items.
    /// </summary>
    public class ForwardViewModel : ViewModelBase
    {
        private Dictionary<string, string> dictParams;

        /// <summary>
        /// A Viewmodel for multi-itembox-items.
        /// </summary>
        /// <param name="dictParams">The params for the itembox settings.</param>
        public ForwardViewModel(Dictionary<string, string> dictParams)
        {
            RaisePropertyChanged(nameof(MultiItemboxItems));
            this.dictParams = dictParams;
            RaisePropertyChanged(nameof(DictParams));
            RaisePropertyChanged(nameof(CommentText));
        }

        /// <summary>
        /// Gets or sets the multi-itembox-items which are the selected items.
        /// </summary>
        public ObservableCollection<IMultiSelectionComboBoxItem> MultiItemboxItems { get; set; } = new ObservableCollection<IMultiSelectionComboBoxItem>();

        /// <summary>
        /// Gets or sets the params which are important for the itembox settings.
        /// </summary>
        public Dictionary<string, string> DictParams { get => dictParams; set => dictParams = value; }

        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        public string CommentText { get; set; }
    }
}