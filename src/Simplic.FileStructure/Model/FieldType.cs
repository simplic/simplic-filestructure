﻿using System;

namespace Simplic.FileStructure
{
    /// <summary>
    /// represents a field type 
    /// </summary>
    public class FieldType
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
        /// Gets or sets the field name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the field datatype
        /// </summary>
        public string Datatype
        {
            get;
            set;
        }
    }
}
