using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Simplic.FileStructure.Workflow.UI
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<NewModel> newModels;

        public MainViewModel()
        {
            newModels = new ObservableCollection<NewModel>();
            newModels.Add(new NewModel() { City = "Stadt", Address = "Address", Region = "Reif" });
            newModels.Add(new NewModel() { City = "Stadt", Address = "Address", Region = "Reif" });
            newModels.Add(new NewModel() { City = "Stadt", Address = "Address", Region = "Reif" });
            newModels.Add(new NewModel() { City = "Stadt", Address = "Address", Region = "Reif" });
            RaisePropertyChanged(nameof(NewModels));
        }

        public ObservableCollection<NewModel> NewModels { get => newModels; set => newModels = value; }
    }
}
