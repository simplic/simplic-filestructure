using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    public enum DocumentWorkflowStateType

    {
        InReview = 0,
        Forwarded = 1,
        ForwardedCopy = 2,
        Completed = 10
    }
}
