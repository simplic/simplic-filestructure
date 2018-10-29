using CommonServiceLocator;
using Simplic.Framework.DBUI;
using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        private ICommand addDocumentPathCommand;
        private ICommand changeDocumentPathCommand;
        private ICommand removeDocumentPathCommand;

        private IList<DocumentPathViewModel> removedPaths;

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

            removedPaths = new List<DocumentPathViewModel>();

            paths = new ObservableCollection<DocumentPathViewModel>();
            foreach (var path in documentPathService.GetByDocumentId(document.Guid))
            {
                var pathVM = new DocumentPathViewModel(path)
                {
                    Parent = this
                };

                paths.Add(pathVM);
            }

            // Add new document path
            addDocumentPathCommand = new RelayCommand((p) => 
            {
                var newDocumentPath = SelectPath();
                var newDocumentPathVM = new DocumentPathViewModel(newDocumentPath)
                {
                    Parent = this
                };

                Paths.Add(newDocumentPathVM);
            });

            // Change document path
            changeDocumentPathCommand = new RelayCommand((p) =>
            {
                var selectedPath = p as DocumentPathViewModel;
                var fileStructure = fileStructureService.Get(selectedPath.Model.FileStructureGuid);
                

                var selectPathWindow = new FileStructureWindow();
                selectPathWindow.Initialize(fileStructure);
                selectPathWindow.IsInSelectMode = true;
                selectPathWindow.ShowDialog();

                if (selectPathWindow.SelectedDirectory != null)
                {
                    selectedPath.Model.DirectoryGuid = selectPathWindow.SelectedDirectory.Id;
                }
            });

            // Remove document path
            removeDocumentPathCommand = new RelayCommand((p) =>
            {
                var messageBoxResult = MessageBox.Show("?", "?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var selectedPath = p as DocumentPathViewModel;

                    removedPaths.Add(selectedPath);
                    paths.Remove(selectedPath);
                }
            });
        }

        /// <summary>
        /// Select document path
        /// </summary>
        /// <returns></returns>
        private FileStructureDocumenPath SelectPath()
        {
            // Show stack selection
            var selectStackItemBox = ItemBoxManager.GetItemBoxFromDB("IB_FileStructure_Stack");
            selectStackItemBox.ShowDialog();

            // Select instance-data
            if (selectStackItemBox.SelectedItem != null)
            {
                var selectDataItemBox = ItemBoxManager.GetItemBoxFromDB(selectStackItemBox.GetSelectedItemCell("StackDataItemBox").ToString());
                selectDataItemBox.ShowDialog();

                if (selectDataItemBox.SelectedItem != null)
                {
                    var fileStructure = fileStructureService.GetByInstanceDataGuid((Guid)selectDataItemBox.GetSelectedItemCell("Guid"));
                    if (fileStructure == null)
                    {
                        MessageBox.Show(":(");
                    }
                    else
                    {
                        var selectPathWindow = new FileStructureWindow();
                        selectPathWindow.Initialize(fileStructure);
                        selectPathWindow.IsInSelectMode = true;
                        selectPathWindow.ShowDialog();

                        if (selectPathWindow.SelectedDirectory != null)
                        {
                            var newDocumentPath = new FileStructureDocumenPath
                            {
                                Id = Guid.NewGuid(),
                                DirectoryGuid = selectPathWindow.SelectedDirectory.Id,
                                DocumentGuid = document.Guid,
                                FileStructureGuid = fileStructure.Id
                            };

                            return newDocumentPath;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public void Save()
        {
            // Remove path
            foreach (var path in removedPaths)
                documentPathService.Delete(path.Model);

            removedPaths.Clear();

            foreach (var path in paths)
                documentPathService.Save(path.Model);

            IsDirty = false;
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

        /// <summary>
        /// Gets or sets the add document path command
        /// </summary>
        public ICommand AddDocumentPathCommand
        {
            get
            {
                return addDocumentPathCommand;
            }

            set
            {
                addDocumentPathCommand = value;
            }
        }

        /// <summary>
        /// Gets or sets the change document path command
        /// </summary>
        public ICommand ChangeDocumentPathCommand
        {
            get
            {
                return changeDocumentPathCommand;
            }

            set
            {
                changeDocumentPathCommand = value;
            }
        }

        /// <summary>
        /// Gets or sets the remove document path command
        /// </summary>
        public ICommand RemoveDocumentPathCommand
        {
            get
            {
                return removeDocumentPathCommand;
            }

            set
            {
                removeDocumentPathCommand = value;
            }
        }
    }
}
