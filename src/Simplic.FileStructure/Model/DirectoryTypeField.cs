using System;

namespace Simplic.FileStructure
{
    public class DirectoryTypeField
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

        public Guid DirectoryTypeId
        {
            get;
            set;
        }
    }
}
