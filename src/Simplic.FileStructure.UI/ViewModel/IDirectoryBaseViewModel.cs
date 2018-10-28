using System.Collections.ObjectModel;

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
