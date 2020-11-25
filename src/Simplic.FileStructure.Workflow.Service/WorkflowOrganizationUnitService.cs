using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.FileStructure.Workflow;

namespace Simplic.FileStructure.Workflow.Service
{
    /// <summary>
    /// Represents the default implementation of <see cref="IWorkflowOrganizationUnitService"/>
    /// </summary>
    public class WorkflowOrganizationUnitService : IWorkflowOrganizationUnitService
    {

        private readonly IWorkflowOrganizationUnitRepository repository;

        /// <summary>
        /// Constructor to pass an implementation of <see cref="IWorkflowOrganizationUnitRepository"/>
        /// </summary>
        /// <param name="repository"></param>
        public WorkflowOrganizationUnitService(IWorkflowOrganizationUnitRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Deletes a <see cref="WorkflowOrganizationUnit"/> using an implementation of <see cref="IWorkflowOrganizationUnitRepository"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Delete(WorkflowOrganizationUnit obj) => repository.Delete(obj);

        /// <summary>
        /// Deletes <see cref="WorkflowOrganizationUnit"/> using an implementation of <see cref="IWorkflowOrganizationUnitRepository"/> based on id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Delete(Guid id) => repository.Delete(id);

        /// <summary>
        /// Gets a <see cref="WorkflowOrganizationUnit"/> based on id using a implementation of <see cref="IWorkflowOrganizationUnitRepository"/> o
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public WorkflowOrganizationUnit Get(Guid id) => repository.Get(id);

        /// <summary>
        /// Gets all of <see cref="WorkflowOrganizationUnit"/> using a implementation of <see cref="IWorkflowOrganizationUnitRepository"/> 
        /// </summary>
        /// <returns>A enumerable of all units</returns>
        public IEnumerable<WorkflowOrganizationUnit> GetAll() => repository.GetAll();

        /// <summary>
        /// Saves the <see cref="WorkflowOrganizationUnit"/> based on the obj using a implementation of <see cref="IWorkflowOrganizationUnitRepository"/>
        /// </summary>
        /// <param name="obj">The object to save</param>
        /// <returns></returns>
        public bool Save(WorkflowOrganizationUnit obj) => repository.Save(obj);
    }
}
