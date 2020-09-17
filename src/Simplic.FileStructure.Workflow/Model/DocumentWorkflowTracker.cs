using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    public class DocumentWorkflowTracker
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Guid DocumentId { get; set; }
        public int UserId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public int? TargetUserId { get; set; }

        public DocumentWorkflowStateType ActionName { get; set; }
    }
}
