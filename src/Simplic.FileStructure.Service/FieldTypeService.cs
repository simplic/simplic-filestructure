﻿using System;
using System.Collections.Generic;

namespace Simplic.FileStructure.Service
{
    /// <summary>
    /// Field type service implementation
    /// </summary>
    public class FieldTypeService : IFieldTypeService
    {
        private readonly IFieldTypeRepository repository;

        /// <summary>
        /// Initialize service
        /// </summary>
        /// <param name="repository">Repository instance</param>
        public FieldTypeService(IFieldTypeRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Delete field type
        /// </summary>
        /// <param name="id">Field type id</param>
        /// <returns>True if successful</returns>
        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Delete field type
        /// </summary>
        /// <param name="obj">Type instance</param>
        /// <returns>True if successfull</returns>
        public bool Delete(FieldType obj)
        {
            return repository.Delete(obj);
        }

        /// <summary>
        /// Get field type by id
        /// </summary>
        /// <param name="id">Unique id</param>
        /// <returns>Field type instance</returns>
        public FieldType Get(Guid id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Get all field types
        /// </summary>
        /// <returns>Enumerable of type instance</returns>
        public IEnumerable<FieldType> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Save the field type
        /// </summary>
        /// <param name="obj">Field type instance</param>
        /// <returns>True if successfull</returns>
        public bool Save(FieldType obj)
        {
            return repository.Save(obj);
        }

    }
}
