using System;

namespace Simplic.FileStructure
{
    public class DirectoryField
    {
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();

        public Guid FieldTypeId
        {
            get;
            set;
        }

        public Guid DirectoryId
        {
            get;
            set;
        }

        public String Value
        {
            get;
            set;
        }
    }
}
