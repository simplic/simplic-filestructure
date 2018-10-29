using Simplic.UI.MVC;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Document path viewmodel
    /// </summary>
    public class DocumentPathViewModel : ViewModelBase
    {
        private FileStructureDocumenPath path;
        private ObservableCollection<FrameworkElement> visualPathElements;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="path">Path instance</param>
        public DocumentPathViewModel(FileStructureDocumenPath path)
        {
            this.path = path;

            VisualPathElements = new ObservableCollection<FrameworkElement>();
            VisualPathElements.Add(new Label { Content = "TEST" });
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

        /// <summary>
        /// Gets or sets the visual path elements
        /// </summary>
        public ObservableCollection<FrameworkElement> VisualPathElements
        {
            get
            {
                return visualPathElements;
            }

            set
            {
                visualPathElements = value;
            }
        }
    }
}
