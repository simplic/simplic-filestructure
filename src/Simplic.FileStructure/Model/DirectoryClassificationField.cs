using System;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Represents the allowed field types for a Directory Classification 
    /// </summary>
    public class DirectoryClassificationField
    {
        /// <summary>
        /// Guid of the Directory Classification to Field
        /// </summary>
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();

        /// <summary>
        /// Id of the Directory Classification
        /// </summary>
        public Guid DirectoryClassificationId
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the Field Type
        /// </summary>
        public Guid FieldTypeId
        {
            get;
            set;
        }
    }
}
