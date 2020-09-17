using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    /// <summary>
    /// Represents the implementation of the interface <see cref="IDocumentWorkflowAssignmentService"/>
    /// </summary>
    public class DocumentWorkflowAssignmentService : IDocumentWorkflowAssignmentService
    {
       
        private readonly IDocumentWorkflowAssignmentRepository repository;
        public DocumentWorkflowAssignmentService(IDocumentWorkflowAssignmentRepository repository)
        {
            this.repository = repository;
        }

        #region[IRepositoryBase]
        
        public bool Delete(DocumentWorkflowAssignment obj) => repository.Delete(obj);

        public bool Delete(long id) => repository.Delete(id);

        public DocumentWorkflowAssignment Get(long id) => repository.Get(id);
        

        public IEnumerable<DocumentWorkflowAssignment> GetAll() => repository.GetAll();
        

        public bool Save(DocumentWorkflowAssignment obj) => repository.Save(obj);
        
        #endregion

        public bool Exists(Guid documentId, Guid workflowId) => repository.Exists(documentId, workflowId);

        public void SetState(Guid documentId, Guid workflowId, Guid stateId) => repository.SetState(documentId, workflowId, stateId);
    }
}
