using Simplic.FileStructure.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DocumentWorkflowAppSettingsService : IDocumentWorkflowAppSettingsService
    {
        IDocumentWorkflowAppSettingsRepository repository;
        public DocumentWorkflowAppSettingsService(IDocumentWorkflowAppSettingsRepository repository)
        {
            this.repository = repository;
        }


        public bool Delete(DocumentWorkflowAppSettings obj)
        {
            return repository.Delete(obj);
        }

        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        public DocumentWorkflowAppSettings Get(Guid id)
        {
            return repository.Get(id);
        }

        public IEnumerable<DocumentWorkflowAppSettings> GetAll()
        {
            return repository.GetAll();
        }

        public bool Save(DocumentWorkflowAppSettings obj)
        {
            return repository.Save(obj);
        }
    }
}
