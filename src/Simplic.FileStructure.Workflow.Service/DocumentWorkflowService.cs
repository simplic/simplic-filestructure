using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DocumentWorkflowService : IDocumentWorkflowContextService
    {
        private IDocumentWorkflowContextRepository repository;

        public DocumentWorkflowService(IDocumentWorkflowContextRepository repository)
        {
            this.repository = repository;
        }


        public bool Delete(DocumentWorkflowContext obj)
        {
            return repository.Delete(obj);
        }

        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        public DocumentWorkflowContext Get(Guid id)
        {
            return repository.Get(id);
        }

        public IEnumerable<DocumentWorkflowContext> GetAll()
        {
            return repository.GetAll();
        }

        public bool Save(DocumentWorkflowContext obj)
        {
            return repository.Save(obj);
        }
    }
}
