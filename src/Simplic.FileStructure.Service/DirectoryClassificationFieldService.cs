using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.FileStructure.Service
{
    /// <summary>
    /// Directory type service implementation
    /// </summary>
    public class DirectoryClassificationFieldService : IDirectoryClassificationFieldService
    {
        private readonly IDirectoryClassificationFieldRepository repository;

        /// <summary>
        /// Initialize service
        /// </summary>
        /// <param name="repository">Repository instance</param>
        public DirectoryClassificationFieldService(IDirectoryClassificationFieldRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Delete Directory classification field by Id
        /// </summary>
        /// <param name="id">Directory classification field id</param>
        /// <returns>True if successful</returns>
        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Delete Directory classification field
        /// </summary>
        /// <param name="obj">Directory classification field instance</param>
        /// <returns>True if successfull</returns>
        public bool Delete(DirectoryClassificationField obj)
        {
            return repository.Delete(obj);
        }

        /// <summary>
        /// Get Directory classification field by id
        /// </summary>
        /// <param name="id">Unique id</param>
        /// <returns>Directory type instance</returns>
        public DirectoryClassificationField Get(Guid id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Get all Directory classification fields
        /// </summary>
        /// <returns>Enumerable of type instance</returns>
        public IEnumerable<DirectoryClassificationField> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public IEnumerable<DirectoryClassificationField> GetByDirectoryClassificationId(Guid guid)
        {
            return repository.GetByDirectoryClassificationId(guid);
        }

        /// <summary>
        /// Save the directory type
        /// </summary>
        /// <param name="obj">Directory type instance</param>
        /// <returns>True if successfull</returns>
        public bool Save(DirectoryClassificationField obj)
        {
            return repository.Save(obj);
        }

        /// <summary>
        /// Saves an Array of allowed fieldtypes for a given DirectoryClassification
        /// </summary>
        /// <param name="fieldTypes">The allowed field types</param>
        /// <param name="notChosenFieldTypes">The not allowed field types</param>
        /// <param name="directoryClassification">The directory classification for which the field types are meant for</param>
        /// <returns></returns>
        public bool SaveFieldTypes(List<FieldType> fieldTypes, List<FieldType> notChosenFieldTypes, DirectoryClassification dirClassification)
        {
            var existingTypes = GetAll();

            foreach(var type in notChosenFieldTypes)
            {
                var x = existingTypes.FirstOrDefault(ex => ex.DirectoryClassificationId == dirClassification.Id && ex.FieldTypeId == type.Id);
                if (x != null)
                    Delete(x);
            }

            foreach (var type in fieldTypes)
            {
                var x = existingTypes.FirstOrDefault(ex => ex.DirectoryClassificationId == dirClassification.Id && ex.FieldTypeId == type.Id);
                if (x == null)
                {
                    var dirField = new DirectoryClassificationField()
                    {
                        DirectoryClassificationId = dirClassification.Id,
                        FieldTypeId = type.Id
                    };
                    Save(dirField);
                }
            }

            return true;
        }

        /// <summary>
        /// Deletes all entries for a given DirectoryClassification
        /// </summary>
        /// <param name="directoryClassification"></param>
        /// <returns></returns>
        public bool DeleteAll(DirectoryClassification directory)
        {
            foreach (var dirField in repository.GetAll().Where(df => df.DirectoryClassificationId == directory.Id))
            {
                Delete(dirField);
            }
            return true;
        }
    }
}
