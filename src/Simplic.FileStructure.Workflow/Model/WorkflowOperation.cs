using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    public class WorkflowOperation
    {
        public Guid Guid { get; set; }

        public string InternalWorkflowName { get; set; }

        public int UserId { get; set; }

        public int TargetUserId { get; set; }

        public string UserName { get; set; }

        public Guid DocumentId { get; set; }
        
        public Guid DocumentPath { get; set; }

        public DateTime CreateDateTime { get; set; }
        
        public DateTime UpdateDateTime { get; set; }

        public string ActionName { get; set; }


    }
}
