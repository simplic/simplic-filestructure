using System;

namespace Simplic.FileStructure
{
    /// <summary>
    /// Represents the value of a field type for a direcotry
    /// </summary>
    public class DirectoryField
    {
        /// <summary>
        /// Gets or sets the GUID
        /// </summary>
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the Field type Id
        /// </summary>
        public Guid FieldTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the directory Id
        /// </summary>
        public Guid DirectoryId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value as string if the Field type is a Text
        /// </summary>
        public String StringValue { get; set; }

        /// <summary>
        /// Gets or sets the value as a Date if the Field type is a Date
        /// </summary>
        public DateTime? DateValue { get; set; }

        /// <summary>
        /// Gets or sets the value as a floating point number if the Field type is a number
        /// </summary>
        public Double? NumericValue { get; set; }

        /// <summary>
        /// Gets or sets the value as a boolean if the Field type is a boolean
        /// </summary>
        public Boolean? BooleanValue { get; set; }
    }
}
