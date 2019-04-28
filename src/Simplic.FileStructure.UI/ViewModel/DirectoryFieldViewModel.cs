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
    /// File structure editor viewmodel
    /// </summary>
    public class DirectoryFieldViewModel : ExtendableViewModel
    {
        private List<FieldType> availableFieldTypes;

        private Directory directory;
        private readonly IDirectoryTypeFieldService directoryTypeFieldService;
        private readonly IDirectoryFieldService directoryFieldService;
        private readonly IFieldTypeService fieldTypeService;
        /// <summary>
        /// Create view model
        /// </summary>
        public DirectoryFieldViewModel()
        {
            directoryTypeFieldService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryTypeFieldService>();
            directoryFieldService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDirectoryFieldService>();
            fieldTypeService = CommonServiceLocator.ServiceLocator.Current.GetInstance<IFieldTypeService>();
        }

        public void Save()
        {
            foreach (var entry in GridEntries)
            {
                if(entry.Found && entry.IsInherited && entry.Content != OriginalGridEntries.First(o => o.Name == entry.Name).Content)
                {
                    var dirField = new DirectoryField()
                    {
                        DirectoryId = directory.Id,
                        FieldTypeId = entry.FieldTypeId,
                        Value = entry.Content
                    };

                    directoryFieldService.Save(dirField);
                }
                else if(entry.Found && !entry.IsInherited && entry.Content != OriginalGridEntries.First(o => o.Name == entry.Name).Content)
                {
                    var dirField = new DirectoryField()
                    {
                        Id = entry.EntryId.Value,
                        DirectoryId = directory.Id,
                        FieldTypeId = entry.FieldTypeId,
                        Value = entry.Content
                    };


                    if (String.IsNullOrEmpty(entry.Content))
                        directoryFieldService.Delete(dirField);
                    else
                        directoryFieldService.Save(dirField);
                }
                else if(!entry.Found && !String.IsNullOrEmpty(entry.Content))
                {
                    var dirField = new DirectoryField()
                    {
                        DirectoryId = directory.Id,
                        FieldTypeId = entry.FieldTypeId,
                        Value = entry.Content
                    };

                    directoryFieldService.Save(dirField);
                }
            }
        }


        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="model">Model to initialize</param>
        public void Initialize(Directory dir)
        {
            this.directory = dir;
            GridEntries = new List<GridEntry>();

            foreach (var field in AvailableFieldTypes)
            {
                var gridEntry = new GridEntry();
                gridEntry = CreateEntry(directory, field, gridEntry);
                GridEntries.Add(gridEntry);
            }

            OriginalGridEntries = new List<GridEntry>(GridEntries.Select(g => (GridEntry) g.Clone()).ToList());
        }

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

            gridEntry.Content = dirField?.Value;
            gridEntry.EntryId = dirField?.Id;
            gridEntry.Name = field.Name;
            gridEntry.FieldTypeId = field.Id;
            gridEntry.DateVisible = field.Datatype == "DateTime" ? Visibility.Visible : Visibility.Collapsed;
            gridEntry.TextVisible = field.Datatype == "string" ? Visibility.Visible : Visibility.Collapsed;
            gridEntry.NumberVisible = field.Datatype == "int" ? Visibility.Visible : Visibility.Collapsed;

            return gridEntry;
        }

        public List<GridEntry> GridEntries
        {
            get;
            set;
        }

        public List<GridEntry> OriginalGridEntries
        {
            get;
            set;
        }

        public List<FieldType> AvailableFieldTypes
        {
            get
            {
                if(availableFieldTypes == null)
                {
                    var dirFieldTypes = directoryTypeFieldService.GetByDirectoryTypeId(directory.DirectoryTypeId.ToString());
                    availableFieldTypes = new List<FieldType>();
                    foreach (var dirFieldType in dirFieldTypes)
                    {
                        availableFieldTypes.Add(fieldTypeService.Get(dirFieldType.FieldTypeId));
                    }
                }
                return availableFieldTypes;
            }
        }

        public class GridEntry : ICloneable
        {
            public Guid? EntryId { get; set; }
            public Guid FieldTypeId { get; set; }
            public string Name { get; set; }
            public string Content { get; set; }
            public bool Found { get; set; }
            public bool IsInherited { get; set; }

            public Visibility DateVisible { get; set; }
            public Visibility TextVisible { get; set; }
            public Visibility NumberVisible { get; set; }

            public object Clone()
            {
                var entry = new GridEntry()
                {
                    Content = Content,
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
