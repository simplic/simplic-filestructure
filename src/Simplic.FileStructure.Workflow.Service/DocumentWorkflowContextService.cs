using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DocumentWorkflowContextService : IDocumentWorkflowContextService
    {
        IDocumentWorkflowContextRepository repository;
        public DocumentWorkflowContextService(IDocumentWorkflowContextRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Returns a bool which is true if the delete based on an obj is executed
        /// </summary>
        /// <param name="obj">document workflow context</param>
        /// <returns></returns>
        public bool Delete(DocumentWorkflowContext obj)
        {
            return repository.Delete(obj);
        }

        /// <summary>
        /// Returns a bool which is true if the delete based on a id is executed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Returns the poco DocumentWorkflowContext based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DocumentWorkflowContext Get(Guid id)
        {
            return repository.Get(id);
        }

        /// <summary>
        /// Returns an IEnumarable which contains all DocumentWorkflowContext
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DocumentWorkflowContext> GetAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Returns a bool which is true if the save is executed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Save(DocumentWorkflowContext obj)
        {
            return repository.Save(obj);
        }
    }
}
