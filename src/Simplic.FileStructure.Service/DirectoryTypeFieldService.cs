using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.FileStructure.Service
{
    /// <summary>
    /// Directory type service implementation
    /// </summary>
    public class DirectoryTypeFieldService : IDirectoryTypeFieldService
    {
        private readonly IDirectoryTypeFieldRepository repository;

        /// <summary>
        /// Initialize service
        /// </summary>
        /// <param name="repository">Repository instance</param>
        public DirectoryTypeFieldService(IDirectoryTypeFieldRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Delete directory type
        /// </summary>
        /// <param name="id">Directory type id</param>
        /// <returns>True if successful</returns>
        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Delete directory type
        /// </summary>
        /// <param name="obj">Type instance</param>
        /// <returns>True if successfull</returns>
        public bool Delete(DirectoryTypeField obj)
        {
            return repository.Delete(obj);
        }

        /// <summary>
        /// Get directory type by id
        /// </summary>
        /// <param name="id">Unique id</param>
        /// <returns>Directory type instance</returns>
        public DirectoryTypeField Get(Guid id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Get all directory types
        /// </summary>
        /// <returns>Enumerable of type instance</returns>
        public IEnumerable<DirectoryTypeField> GetAll()
        {
            return repository.GetAll();
        }

        public IEnumerable<DirectoryTypeField> GetByDirectoryTypeId(string guid)
        {
            return repository.GetByDirectoryTypeId(guid);
        }

        /// <summary>
        /// Save the directory type
        /// </summary>
        /// <param name="obj">Directory type instance</param>
        /// <returns>True if successfull</returns>
        public bool Save(DirectoryTypeField obj)
        {
            return repository.Save(obj);
        }

        public bool SaveFieldTypes(List<FieldType> fieldTypes, List<FieldType> notChosenFieldTypes, DirectoryType directory)
        {
            var existingTypes = GetAll();

            foreach(var type in notChosenFieldTypes)
            {
                var x = existingTypes.FirstOrDefault(ex => ex.DirectoryTypeId == directory.Id && ex.FieldTypeId == type.Id);
                if (x != null)
                    Delete(x);
            }

            foreach (var type in fieldTypes)
            {
                var x = existingTypes.FirstOrDefault(ex => ex.DirectoryTypeId == directory.Id && ex.FieldTypeId == type.Id);
                if (x == null)
                {
                    var dirField = new DirectoryTypeField()
                    {
                        DirectoryTypeId = directory.Id,
                        FieldTypeId = type.Id
                    };
                    Save(dirField);
                }
            }

            return true;
        }

        public bool DeleteAll(DirectoryType directory)
        {
            foreach (var dirField in repository.GetAll().Where(df => df.DirectoryTypeId == directory.Id))
            {
                Delete(dirField);
            }
            return true;
        }
    }
}
