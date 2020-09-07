using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DocumentWorkflowAssignmentService : IDocumentWorkflowAssignmentService
    {
        private readonly IDocumentWorkflowAssignmentRepository repository;
        public DocumentWorkflowAssignmentService(IDocumentWorkflowAssignmentRepository repository)
        {
            this.repository = repository;
        }

        public bool AlreadyExists(Guid documentId)
        {
            return repository.AlreadyExists(documentId);
        }

        public bool Delete(DocumentWorkflowAssignment obj)
        {
            return repository.Delete(obj);
        }

        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        public DocumentWorkflowAssignment Get(Guid id)
        {
            return repository.Get(id);
        }

        public IEnumerable<DocumentWorkflowAssignment> GetAll()
        {
            return repository.GetAll();
        }

        public bool Save(DocumentWorkflowAssignment obj)
        {
            return repository.Save(obj);
        }
    }
}
