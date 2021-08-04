using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow.Service
{
    public class DocumentWorkflowConfigurationService : IDocumentWorkflowConfigurationService
    {
        private readonly IDocumentWorkflowConfigurationRepository repository;
        private readonly IWorkflowOrganizationUnitAssignmentRepository workflowOrganizationUnitAssignmentRepository;

        public DocumentWorkflowConfigurationService(IDocumentWorkflowConfigurationRepository repository
            , IWorkflowOrganizationUnitAssignmentRepository workflowOrganizationUnitAssignmentRepository)
        {
            this.workflowOrganizationUnitAssignmentRepository = workflowOrganizationUnitAssignmentRepository;
            this.repository = repository;
        }

        /// <summary>
        /// Returns a bool which is true if the delete based on an obj is executed 
        /// </summary>
        /// <param name="obj">document workflow context</param>
        /// <returns></returns>
        public bool Delete(DocumentWorkflowConfiguration obj)
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
        /// Returns the poco DocumentWorkflowConfiguaration based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DocumentWorkflowConfiguration Get(Guid id)
        {
            return LoadDependingData(repository.Get(id));
        }

        /// <summary>
        /// Returns an IEnumarable which contains all DocumentWorkflowContext
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DocumentWorkflowConfiguration> GetAll()
        {
            return repository.GetAll().Select(x => LoadDependingData(x));
        }

        /// <summary>
        /// Load all data, that depends on a <see cref="DocumentWorkflowConfiguration"/>, e.g. organization unit assignemtns
        /// </summary>
        /// <param name="configuration">Configuration instance</param>
        /// <returns>Input configuration instance</returns>
        private DocumentWorkflowConfiguration LoadDependingData(DocumentWorkflowConfiguration configuration)
        {
            if (configuration == null)
                return null;

            var unitAssignments = workflowOrganizationUnitAssignmentRepository.GetByWorkflowId(configuration.Guid);

            configuration.OrganizationUnits = new Collections.Generic.StatefulCollection<WorkflowOrganizationUnitAssignment>(unitAssignments);

            return configuration;
        }

        /// <summary>
        /// Returns a bool which is true if the save is executed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Save(DocumentWorkflowConfiguration obj)
        {
            repository.Save(obj);

            if (obj.OrganizationUnits != null)
            {
                foreach (var assignment in obj.OrganizationUnits.GetRemovedItems())
                    workflowOrganizationUnitAssignmentRepository.Delete(assignment);

                foreach (var assignment in obj.OrganizationUnits.GetNewItems())
                {
                    assignment.WorkflowId = obj.Guid;
                    workflowOrganizationUnitAssignmentRepository.Save(assignment);
                }

                foreach (var assignment in obj.OrganizationUnits.GetItems())
                    workflowOrganizationUnitAssignmentRepository.Save(assignment);

                obj.OrganizationUnits.Commit();
            }

            return true;
        }
    }
}
