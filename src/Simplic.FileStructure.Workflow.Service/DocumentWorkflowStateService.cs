using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DocumentWorkflowStateService : IDocumentWorkflowStateService
    {


        private readonly IDocumentWorkflowStateRepository repository;
        public DocumentWorkflowStateService(IDocumentWorkflowStateRepository repository)
        {
            this.repository = repository;
        }

        #region[IRepositoryBase]
        public bool Delete(DocumentWorkflowState obj) => repository.Delete(obj);

        public bool Delete(Guid id) => repository.Delete(id);

        public DocumentWorkflowState Get(Guid id) => repository.Get(id);

        public IEnumerable<DocumentWorkflowState> GetAll() => repository.GetAll();

        public bool Save(DocumentWorkflowState obj) => repository.Save(obj);
        #endregion
    }
}
