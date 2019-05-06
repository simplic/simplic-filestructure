using Simplic.DataStack;
using Simplic.Framework.UI;
using Simplic.Icon;
using Simplic.Localization;
using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Simplic.FileStructure.UI
{
    /// <summary>
    /// Directory field editor viewmodel
    /// </summary>
    public class DirectoryFieldViewModel : ExtendableViewModel
    {
        private List<FieldType> availableFieldTypes;
        private List<DirectoryClassification> availableDirectoryClassifications;
        private List<GridEntry> gridEntries;

        private Directory directory;
        private FileStructure fileStructure;
        private DirectoryClassification directoryClassification;
        private readonly IDirectoryTypeClassificationService directoryTypeClassificationService;
        private readonly IDirectoryClassificationFieldService directoryTypeFieldService;
        private readonly IDirectoryClassificationService directoryClassificationService;
        private readonly IDirectoryFieldService directoryFieldService;
        private readonly IFieldTypeService fieldTypeService;
        private readonly IFileStructureService fileStructureService;

        /// <summary>
        /// Create view model
        /// </summary>
        public DirectoryFieldViewModel()
        {
            directoryTypeClassificationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryTypeClassificationService>();
            directoryTypeFieldService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryClassificationFieldService>();
            directoryClassificationService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryClassificationService>();
            directoryFieldService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryFieldService>();
            fileStructureService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFileStructureService>();
            fieldTypeService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFieldTypeService>();
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public void Save()
        {
            foreach (var entry in GridEntries)
            {
                if (entry.Found && entry.IsInherited &&
                  ((entry.TextVisible == Visibility.Visible && entry.StringContent != OriginalGridEntries.First(o => o.Name == entry.Name).StringContent) ||
                   (entry.DateVisible == Visibility.Visible && entry.DateContent != OriginalGridEntries.First(o => o.Name == entry.Name).DateContent) ||
                   (entry.NumberVisible == Visibility.Visible && entry.NumericContent != OriginalGridEntries.First(o => o.Name == entry.Name).NumericContent) ||
                   (entry.BooleanVisible == Visibility.Visible && entry.BooleanContent != OriginalGridEntries.First(o => o.Name == entry.Name).BooleanContent)))
                {
                    var dirField = new DirectoryField()
                    {
                        DirectoryId = directory.Id,
                        FieldTypeId = entry.FieldTypeId,
                        StringValue = entry.StringContent,
                        DateValue = entry.DateContent,
                        NumericValue = entry.NumericContent,
                        BooleanValue = entry.BooleanContent
                    };

                    directoryFieldService.Save(dirField);
                }
                else if (entry.Found && !entry.IsInherited &&
                       ((entry.TextVisible == Visibility.Visible && entry.StringContent != OriginalGridEntries.First(o => o.Name == entry.Name).StringContent) ||
                        (entry.DateVisible == Visibility.Visible && entry.DateContent != OriginalGridEntries.First(o => o.Name == entry.Name).DateContent) ||
                        (entry.NumberVisible == Visibility.Visible && entry.NumericContent != OriginalGridEntries.First(o => o.Name == entry.Name).NumericContent) ||
                        (entry.BooleanVisible == Visibility.Visible && entry.BooleanContent != OriginalGridEntries.First(o => o.Name == entry.Name).BooleanContent)))
                {
                    var dirField = new DirectoryField()
                    {
                        Id = entry.EntryId.Value,
                        DirectoryId = directory.Id,
                        FieldTypeId = entry.FieldTypeId,
                        StringValue = entry.StringContent,
                        DateValue = entry.DateContent,
                        NumericValue = entry.NumericContent,
                        BooleanValue = entry.BooleanContent
                    };

                    if (String.IsNullOrEmpty(entry.StringContent) && entry.DateContent == null && entry.NumericContent == null && entry.BooleanContent == null)
                        directoryFieldService.Delete(dirField);
                    else
                        directoryFieldService.Save(dirField);
                }
                else if(!entry.Found &&
                       ((entry.TextVisible == Visibility.Visible && entry.StringContent != OriginalGridEntries.First(o => o.Name == entry.Name).StringContent) ||
                        (entry.DateVisible == Visibility.Visible && entry.DateContent != OriginalGridEntries.First(o => o.Name == entry.Name).DateContent) ||
                        (entry.NumberVisible == Visibility.Visible && entry.NumericContent != OriginalGridEntries.First(o => o.Name == entry.Name).NumericContent) ||
                        (entry.BooleanVisible == Visibility.Visible && entry.BooleanContent != OriginalGridEntries.First(o => o.Name == entry.Name).BooleanContent)))
                {
                    var dirField = new DirectoryField()
                    {
                        DirectoryId = directory.Id,
                        FieldTypeId = entry.FieldTypeId,
                        StringValue = entry.StringContent,
                        DateValue = entry.DateContent,
                        NumericValue = entry.NumericContent,
                        BooleanValue = entry.BooleanContent
                    };

                    directoryFieldService.Save(dirField);
                }
            }

            
            fileStructure.Directories.First(d => d.Id == directory.Id).DirectoryClassification = DirectoryClassification;
            fileStructureService.Save(fileStructure);
        }


        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="dir">Directory instance</param>
        /// <param name="fileStructure">FileStructure instance</param>
        public void Initialize(Directory dir, FileStructure fileStructure)
        {
            this.directory = dir;
            this.fileStructure = fileStructure;
            InitializeGrid();
        }

        /// <summary>
        /// Prepares informations to display grid properly
        /// </summary>
        private void InitializeGrid()
        {
            if (AvailableFieldTypes == null)
                return;

            var tmpGridEntries = new List<GridEntry>();

            foreach (var field in AvailableFieldTypes)
            {
                var gridEntry = new GridEntry();
                gridEntry = CreateEntry(directory, field, gridEntry);
                tmpGridEntries.Add(gridEntry);
            }

            GridEntries = tmpGridEntries;

            OriginalGridEntries = new List<GridEntry>(GridEntries.Select(g => (GridEntry)g.Clone()).ToList());
        }

        /// <summary>
        /// Creates a single row for the grid
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="field"></param>
        /// <param name="gridEntry"></param>
        /// <returns></returns>
        private GridEntry CreateEntry(Directory directory, FieldType field, GridEntry gridEntry)
        {
            DirectoryField dirField;
            Directory original = directory;
            do
            {
                dirField = directoryFieldService.Get(directory.Id, field.Id);

                if (dirField == null)
                    directory = directory.Parent;

            } while (directory != null && dirField == null);

            if (directory != null && original.Id == directory.Id)
            {
                gridEntry.IsInherited = false;
                gridEntry.Found = true;
            }
            else
            {
                if (dirField == null)
                    gridEntry.Found = false;
                else
                    gridEntry.Found = true;

                gridEntry.IsInherited = true;
            }


            gridEntry.StringContent = dirField?.StringValue;
            gridEntry.DateContent = dirField?.DateValue;
            gridEntry.NumericContent = dirField?.NumericValue;
            gridEntry.BooleanContent = dirField?.BooleanValue;

            gridEntry.EntryId = dirField?.Id;
            gridEntry.Name = field.Name;
            gridEntry.FieldTypeId = field.Id;
            gridEntry.DateVisible = field.Datatype == "DateTime" ? Visibility.Visible : Visibility.Collapsed;
            gridEntry.TextVisible = field.Datatype == "string" ? Visibility.Visible : Visibility.Collapsed;
            gridEntry.NumberVisible = field.Datatype == "int" ? Visibility.Visible : Visibility.Collapsed;
            gridEntry.BooleanVisible = field.Datatype == "bool" ? Visibility.Visible : Visibility.Collapsed;
            gridEntry.NameVisible = gridEntry.BooleanVisible == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

            return gridEntry;
        }

        /// <summary>
        /// Gets or sets the entries for the grid
        /// </summary>
        public List<GridEntry> GridEntries
        {
            get
            {
                if (gridEntries == null)
                    InitializeGrid();

                return gridEntries;
            }
            set
            {
                PropertySetter(value, (newValue) => { gridEntries = newValue; });
            }
        }

        /// <summary>
        /// A backup copy of the original grid entries, to detect changes to inherited values
        /// </summary>
        public List<GridEntry> OriginalGridEntries
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Directory Classification and refreshes Grid if it got changed
        /// </summary>
        public DirectoryClassification DirectoryClassification
        {
            get
            {
                if (directoryClassification == null)
                    directoryClassification = directory.DirectoryClassification;
                return directoryClassification;
            }
            set
            {
                PropertySetter(value, (newValue) => { directoryClassification = newValue; });
                SetAvailableFieldTypes();
                InitializeGrid();
            }
        }

        /// <summary>
        /// Gets a List of available directory classifications for the directory instance
        /// </summary>
        public List<DirectoryClassification> AvailableDirectoryClassifications
        {
            get
            {
                if (availableDirectoryClassifications == null)
                {
                    var availableDirTypeClassificiations = directoryTypeClassificationService.GetByDirectoryTypeId(directory.DirectoryTypeId);
                    availableDirectoryClassifications = new List<DirectoryClassification>();
                    foreach(var classification in availableDirTypeClassificiations)
                    {
                        availableDirectoryClassifications.Add(directoryClassificationService.Get(classification.DirectoryClassificationId));
                    }
                }

                return availableDirectoryClassifications;
             }
        }

        /// <summary>
        /// Gets a List of available field types for the directory classification
        /// </summary>
        public List<FieldType> AvailableFieldTypes
        {
            get
            {
                if(availableFieldTypes == null)
                {
                    SetAvailableFieldTypes();
                }
                return availableFieldTypes;
            }
            set
            {
                PropertySetter(value, (newValue) => { availableFieldTypes = newValue; });
            }
        }

        /// <summary>
        /// Sets available field types by directory classification from database
        /// </summary>
        private void SetAvailableFieldTypes()
        {
            if (DirectoryClassification == null)
                return;
            var dirFieldTypes = directoryTypeFieldService.GetByDirectoryClassificationId(DirectoryClassification.Id);
            availableFieldTypes = new List<FieldType>();
            foreach (var dirFieldType in dirFieldTypes)
            {
                availableFieldTypes.Add(fieldTypeService.Get(dirFieldType.FieldTypeId));
            }
        }

        /// <summary>
        /// represents a single row in the grid
        /// </summary>
        public class GridEntry : ICloneable
        {
            public Guid? EntryId { get; set; }
            public Guid FieldTypeId { get; set; }
            public string Name { get; set; }
            public string StringContent { get; set; }
            public DateTime? DateContent { get; set; }
            public double? NumericContent { get; set; }
            public bool? BooleanContent { get; set; }
            public bool Found { get; set; }
            public bool IsInherited { get; set; }

            public Visibility DateVisible { get; set; }
            public Visibility TextVisible { get; set; }
            public Visibility NumberVisible { get; set; }
            public Visibility BooleanVisible { get; set; }
            public Visibility NameVisible { get; set; }

            public object Clone()
            {
                var entry = new GridEntry()
                {
                    StringContent = StringContent,
                    DateContent = DateContent,
                    NumericContent = NumericContent,
                    BooleanContent = BooleanContent,
                    EntryId = EntryId,
                    FieldTypeId = FieldTypeId,
                    Found = Found,
                    IsInherited = IsInherited,
                    Name = Name
                };

                return entry;
            }
        }
    }
}
