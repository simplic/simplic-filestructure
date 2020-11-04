using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.FileStructure.Workflow;

namespace Simplic.FileStructure.Workflow.Service
{
    public class WorkflowOrganizationUnitService : IWorkflowOrganizationUnitService
    {

        private readonly IWorkflowOrganizationUnitRepository repository;

        public WorkflowOrganizationUnitService(IWorkflowOrganizationUnitRepository repository)
        {
            this.repository = repository;
        }

        public bool Delete(WorkflowOrganizationUnit obj) => repository.Delete(obj);

        public bool Delete(Guid id) => repository.Delete(id);

        public WorkflowOrganizationUnit Get(Guid id) => repository.Get(id);

        public IEnumerable<WorkflowOrganizationUnit> GetAll() => repository.GetAll();

        public bool Save(WorkflowOrganizationUnit obj) => repository.Save(obj);
    }
}
