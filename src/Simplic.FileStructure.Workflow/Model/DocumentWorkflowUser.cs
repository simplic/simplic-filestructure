using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    public class DocumentWorkflowUser
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
