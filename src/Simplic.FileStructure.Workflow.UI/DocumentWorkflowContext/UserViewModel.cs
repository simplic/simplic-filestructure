using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.UI
{
    public class UserViewModel
    {
        public string Header { get; set; }

        public ObservableCollection<object> Users{ get; set; }
        public ObservableCollection<object> SelectedUsers{ get; set; }
    }
}
