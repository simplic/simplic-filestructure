using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DocumentWorkflowTrackerService : IDocumentWorkflowTrackerService
    {
        private readonly IDocumentWorkflowTrackerRepository repository;

        public DocumentWorkflowTrackerService(IDocumentWorkflowTrackerRepository repository)
        {
            this.repository = repository;
        }

        public bool Delete(DocumentWorkflowTracker obj)
        {
            return repository.Delete(obj);
        }

        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        public DocumentWorkflowTracker Get(Guid id)
        {
            return repository.Get(id);
        }

        public IEnumerable<DocumentWorkflowTracker> GetAll()
        {
            return repository.GetAll();
        }

        ///<inheritdoc/>
        public bool IsDocumentUserAssigned(Guid documentId, int userId)
        {
            return repository.IsDocumentUserAssigned(documentId, userId);
        }

        public bool Save(DocumentWorkflowTracker obj)
        {
            return repository.Save(obj);
        }
    }
}
