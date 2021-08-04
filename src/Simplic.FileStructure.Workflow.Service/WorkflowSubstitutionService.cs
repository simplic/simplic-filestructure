using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class WorkflowSubstitutionService : IWorkflowSubstitutionService
    {
        private readonly IWorkflowSubstitutionRepository repository;
        public WorkflowSubstitutionService(IWorkflowSubstitutionRepository repository)
        {
            this.repository = repository;
        }

        public bool SubstitutionExists(long currentUserId, long absenceUserId) => repository.SubstitutionExists(currentUserId, absenceUserId);
    }
}
