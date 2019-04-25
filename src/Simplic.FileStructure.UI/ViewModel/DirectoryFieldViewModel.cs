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

        }


        /// <summary>
        /// Initialize viewmodel
        /// </summary>
        /// <param name="model">Model to initialize</param>
        public void Initialize(Directory dir)
        {
            this.directory = dir;
            GridEntries = new List<Test>();

            foreach (var field in AvailableFieldTypes)
            {
                var gridEntry = new Test();
                gridEntry.Name = field.Name;
                gridEntry.Content = SearchForValues(directory, field, out gridEntry.IsInherited, out gridEntry.Found);
                MessageBox.Show(
                    "Name: " + gridEntry.Name + Environment.NewLine + 
                    "Content: " + gridEntry.Content + Environment.NewLine + 
                    "IsInherited: " + gridEntry.IsInherited + Environment.NewLine + 
                    "IsFound: " + gridEntry.Found);
                GridEntries.Add(gridEntry);
            }
        }

        private string SearchForValues(Directory directory, FieldType field, out bool isInherited, out bool found)
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
                isInherited = false;
                found = true;
            }
            else
            {
                if (dirField == null)
                    found = false;
                else
                    found = true;

                isInherited = true;
            }

            return dirField?.Value;
        }

        public List<Test> GridEntries
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

        public class Test
        {
            public string Name;
            public bool IsInherited;
            public bool Found;
            public string Content;
        }
    }
}
