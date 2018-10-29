using Simplic.UI.MVC;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Document path viewmodel
    /// </summary>
    public class DocumentPathViewModel : ViewModelBase
    {
        private FileStructureDocumenPath path;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="path">Path instance</param>
        public DocumentPathViewModel(FileStructureDocumenPath path)
        {
            this.path = path;
        }

        /// <summary>
        /// Gets the current document path model
        /// </summary>
        public FileStructureDocumenPath Model
        {
            get
            {
                return path;
            }
        }
    }
}
