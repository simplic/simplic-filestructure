using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DocumentWorkflowOrganizationUnitAssignmentService : IDocumentWorkflowOrganizationUnitAssignmentService
    {
        private readonly IDocumentWorkflowOrganizationUnitAssignmentRepository repository;

        public DocumentWorkflowOrganizationUnitAssignmentService(IDocumentWorkflowOrganizationUnitAssignmentRepository repository)
        {
            this.repository = repository;
        }

        public bool Delete(DocumentWorkflowOrganizationUnitAssignment obj) => repository.Delete(obj);

        public bool Delete(Guid id) => repository.Delete(id);

        public bool DeleteByIds(Guid documentId, Guid organizationId) => repository.DeleteByIds(documentId, organizationId);

        public DocumentWorkflowOrganizationUnitAssignment Get(Guid id) => repository.Get(id);

        public IEnumerable<DocumentWorkflowOrganizationUnitAssignment> GetAll() => repository.GetAll();

        public bool Save(DocumentWorkflowOrganizationUnitAssignment obj) => repository.Save(obj);
    }
}
