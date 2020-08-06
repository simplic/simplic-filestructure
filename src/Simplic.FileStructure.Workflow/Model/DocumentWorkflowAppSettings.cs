using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    public class DocumentWorkflowAppSettings
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string InternalName { get; set; }
        public string PublicName { get; set; }
    }
}
