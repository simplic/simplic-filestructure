using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Cache;
using Simplic.Data.Sql;
using Simplic.Sql;

namespace Simplic.FileStructure.Workflow.Data.DB
{
    public class WorkflowOrganizationUnitAssignmentRepository : SqlRepositoryBase<Guid, WorkflowOrganizationUnitAssignment>, IWorkflowOrganizationUnitAssignmentRepository
    {
        private readonly ISqlService sqlService;
        private readonly IWorkflowOrganizationUnitUserAssignmentRepository workflowOrganizationUnitUserAssignmentRepository;

        public WorkflowOrganizationUnitAssignmentRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService
            , IWorkflowOrganizationUnitUserAssignmentRepository workflowOrganizationUnitUserAssignmentRepository) : base(sqlService, sqlColumnService, cacheService)
        {
            this.workflowOrganizationUnitUserAssignmentRepository = workflowOrganizationUnitUserAssignmentRepository;
            UseCache = true;
            this.sqlService = sqlService;
        }

        public override string TableName => "FileStructure_WorkflowOrganizationUnit_Assignment";

        public override string PrimaryKeyColumn => "Guid";

        public IEnumerable<WorkflowOrganizationUnitAssignment> GetByWorkflowId(Guid guid) => GetAllByColumn("WorkflowId", guid);

        protected override IEnumerable<WorkflowOrganizationUnitAssignment> GetAllByColumn<T>(string columnName, T id)
        {
            return base.GetAllByColumn(columnName, id).Select(x => LoadDepenedingData(x));
        }

        protected override WorkflowOrganizationUnitAssignment GetByColumn<T>(string columnName, T id)
        {
            return LoadDepenedingData(base.GetByColumn(columnName, id));
        }

        private WorkflowOrganizationUnitAssignment LoadDepenedingData(WorkflowOrganizationUnitAssignment unitAssignment)
        {
            if (unitAssignment == null)
                return null;

            var users = workflowOrganizationUnitUserAssignmentRepository.GetByAssignmentId(unitAssignment.Guid);
            unitAssignment.Users = new Collections.Generic.StatefulCollection<WorkflowOrganizationUnitUserAssignment>(users);

            return unitAssignment;
        }


        /// <summary>
        /// Gets all user assignments for a specific workflow and organization
        /// </summary>
        /// <param name="workflowConfigurationId">Unique workflow id</param>
        /// <param name="organizationUnitId">Unique organization id</param>
        /// <returns>List of user-assignments</returns>
        public IEnumerable<WorkflowOrganizationUnitUserAssignment> GetForOrganizationByConfigurationId(Guid workflowConfigurationId, Guid organizationUnitId)
        {
            return workflowOrganizationUnitUserAssignmentRepository.GetForOrganizationByConfigurationId(workflowConfigurationId, organizationUnitId);
        }

        public override bool Save(WorkflowOrganizationUnitAssignment obj)
        {
            base.Save(obj);

            if (obj.Users != null)
            {
                foreach (var assignment in obj.Users.GetRemovedItems())
                    workflowOrganizationUnitUserAssignmentRepository.Delete(assignment);

                foreach (var assignment in obj.Users.GetNewItems())
                    workflowOrganizationUnitUserAssignmentRepository.Save(assignment);

                foreach (var assignment in obj.Users.GetItems())
                    workflowOrganizationUnitUserAssignmentRepository.Save(assignment);

                obj.Users.Commit();
            }

            return true;
        }

        public override Guid GetId(WorkflowOrganizationUnitAssignment obj) => obj.Guid;
    }
}
