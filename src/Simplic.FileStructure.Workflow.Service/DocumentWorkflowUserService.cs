using Simplic.FileStructure.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DocumentWorkflowUserService : IDocumentWorkflowUserService
    {
        IDocumentWorkflowUserRepository documentWorkflowRepository;
        public DocumentWorkflowUserService(IDocumentWorkflowUserRepository documentWorkflowRepository)
        {
            this.documentWorkflowRepository = documentWorkflowRepository;
        }
        public bool Delete(DocumentWorkflowUser obj)
        {
            return documentWorkflowRepository.Delete(obj);
        }

        public bool Delete(Guid id)
        {
            return documentWorkflowRepository.Delete(id);
        }

        public DocumentWorkflowUser Get(Guid id)
        {
            return documentWorkflowRepository.Get(id);
        }

        public DocumentWorkflowUser Get(string internalName, int userId)
        {
            return documentWorkflowRepository.Get(internalName, userId);
        }

        public IEnumerable<DocumentWorkflowUser> GetAll()
        {
            return documentWorkflowRepository.GetAll();
        }

        public bool Save(DocumentWorkflowUser obj)
        {
            return documentWorkflowRepository.Save(obj);

        }
    }
}
