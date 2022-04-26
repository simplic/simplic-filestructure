using Simplic.UI.Control;
using Simplic.UI.MVC;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Simplic.FileStructure.Workflow.UI
{
    public class MultiItemBoxViewModel : ViewModelBase
    {
        private Dictionary<string, string> dictParams;

        public MultiItemBoxViewModel(Dictionary<string, string> dictParams)
        {
            RaisePropertyChanged(nameof(MultiItemboxItems));
            this.dictParams = dictParams;
            RaisePropertyChanged(nameof(DictParams));
            RaisePropertyChanged(nameof(CommentText));
        }

        public ObservableCollection<IMultiSelectionComboBoxItem> MultiItemboxItems { get; set; } = new ObservableCollection<IMultiSelectionComboBoxItem>();
        public Dictionary<string, string> DictParams { get => dictParams; set => dictParams = value; }
        public string CommentText { get; set; }
    }
}
