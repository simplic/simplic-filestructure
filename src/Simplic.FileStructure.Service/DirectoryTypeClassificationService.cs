using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.FileStructure.Service
{
    /// <summary>
    /// Directory type to classification service implementation
    /// </summary>
    public class DirectoryTypeClassificationService : IDirectoryTypeClassificationService
    {
        private readonly IDirectoryTypeClassificationRepository repository;

        /// <summary>
        /// Initialize service
        /// </summary>
        /// <param name="repository">Repository instance</param>
        public DirectoryTypeClassificationService(IDirectoryTypeClassificationRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Delete directory type classification
        /// </summary>
        /// <param name="id">Directory type classification id</param>
        /// <returns>True if successful</returns>
        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Delete directory type classification
        /// </summary>
        /// <param name="obj">Type instance</param>
        /// <returns>True if successfull</returns>
        public bool Delete(DirectoryTypeClassification obj)
        {
            return repository.Delete(obj);
        }

        /// <summary>
        /// Get directory type classification by id
        /// </summary>
        /// <param name="id">Unique id</param>
        /// <returns>Directory type classification instance</returns>
        public DirectoryTypeClassification Get(Guid id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Get all directory type classifications
        /// </summary>
        /// <returns>Enumerable of classification instance</returns>
        public IEnumerable<DirectoryTypeClassification> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Save the directory type classification
        /// </summary>
        /// <param name="obj">Directory type classification instance</param>
        /// <returns>True if successfull</returns>
        public bool Save(DirectoryTypeClassification obj)
        {
            return repository.Save(obj);
        }

        /// <summary>
        /// Deletes all entries for a given directory type
        /// </summary>
        /// <param name="directoryType"></param>
        /// <returns></returns>
        public bool DeleteAll(DirectoryType directory)
        {
            foreach (var dirField in repository.GetAll().Where(df => df.DirectoryTypeId == directory.Id))
            {
                Delete(dirField);
            }
            return true;
        }

        /// <summary>
        /// Saves an Array of directory classifications for a given directory
        /// </summary>
        /// <param name="directoryClassifications"></param>
        /// <param name="notChosenDirectoryClassifications"></param>
        /// <param name="directoryType"></param>
        /// <returns></returns>
        public bool SaveFieldTypes(List<DirectoryClassification> directoryClassifications, List<DirectoryClassification> notChosenDirectoryClassifications, DirectoryType directoryType)
        {
            var existingTypes = GetAll();

            foreach (var dirClassification in notChosenDirectoryClassifications)
            {
                var x = existingTypes.FirstOrDefault(ex => ex.DirectoryTypeId == directoryType.Id && ex.DirectoryClassificationId == dirClassification.Id);
                if (x != null)
                    Delete(x);
            }

            foreach (var dirClassification in directoryClassifications)
            {
                var x = existingTypes.FirstOrDefault(ex => ex.DirectoryTypeId == directoryType.Id && ex.DirectoryClassificationId == dirClassification.Id);
                if (x == null)
                {
                    var dirField = new DirectoryTypeClassification()
                    {
                        DirectoryTypeId = directoryType.Id,
                        DirectoryClassificationId = dirClassification.Id
                    };
                    Save(dirField);
                }
            }

            return true;
        }

        /// <summary>
        /// Get all by DirectoryTypeId
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>A IEnumrable of <see cref="DirectoryTypeClassification"/></returns>
        public IEnumerable<DirectoryTypeClassification> GetByDirectoryTypeId(Guid guid)
        {
            return repository.GetByDirectoryTypeId(guid);
        }
    }
}
