using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DocumentWorkflowUserPermissionService : IDocumentWorkflowUserPermissionRepository
    {
        private readonly IDocumentWorkflowUserPermissionRepository repository;

        public DocumentWorkflowUserPermissionService(IDocumentWorkflowUserPermissionRepository repository)
        {
            this.repository = repository;
        }

        public bool Delete(DocumentWorkflowUserPermission obj)
        {
            return repository.Delete(obj);
        }

        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        public DocumentWorkflowUserPermission Get(Guid id)
        {
            return repository.Get(id);
        }

        public IEnumerable<DocumentWorkflowUserPermission> GetAll()
        {
            return repository.GetAll();
        }

        public bool Save(DocumentWorkflowUserPermission obj)
        {
            return repository.Save(obj);
        }
    }
}
