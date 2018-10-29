using CommonServiceLocator;
using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Document overview control view model
    /// </summary>
    public class DocumentPathOverViewViewModel : ViewModelBase
    {
        private Document.Document document;
        private ObservableCollection<DocumentPathViewModel> paths;

        private readonly IFileStructureDocumentPathService documentPathService;
        private readonly IFileStructureService fileStructureService;

        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="document">Document</param>
        public DocumentPathOverViewViewModel(Document.Document document)
        {
            this.document = document;

            if (document == null)
                throw new ArgumentNullException(nameof(document), $"Document must not be null in {nameof(DocumentPathOverViewViewModel)}.");

            documentPathService = ServiceLocator.Current.GetInstance<IFileStructureDocumentPathService>();
            fileStructureService = ServiceLocator.Current.GetInstance<IFileStructureService>();

            paths = new ObservableCollection<DocumentPathViewModel>();
            foreach (var path in documentPathService.GetByDocumentId(document.Guid))
            {
                var pathVM = new DocumentPathViewModel(path)
                {
                    Parent = this
                };

                paths.Add(pathVM);
            }
        }

        /// <summary>
        /// Gets or sets the list of document paths
        /// </summary>
        public ObservableCollection<DocumentPathViewModel> Paths
        {
            get
            {
                return paths;
            }

            set
            {
                paths = value;
            }
        }
    }
}
