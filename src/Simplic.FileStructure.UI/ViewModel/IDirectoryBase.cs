using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Directory viewmodel base
    /// </summary>
    public interface IDirectoryBaseViewModel
    {
        /// <summary>
        /// Gets or sets a list of subdirectories
        /// </summary>
        ObservableCollection<DirectoryViewModel> Directories { get; set; }
    }
}
