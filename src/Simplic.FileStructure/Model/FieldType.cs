using System;

namespace Simplic.FileStructure
{
    public class FieldType
    {
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the type name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type name
        /// </summary>
        public string Datatype
        {
            get;
            set;
        }
    }
}
