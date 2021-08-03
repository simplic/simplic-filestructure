using Dapper;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    public class WorkflowSubstitutionRepository : IWorkflowSubstitutionRepository
    {
        ISqlService sqlService;
        public WorkflowSubstitutionRepository(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }

        public bool SubstitutionExists(long currentUserId, long absenceUserId)
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.QueryFirstOrDefault<bool>("SELECT CASE WHEN EXISTS(SELECT * FROM ESS_MS_Intern_User where " +
                    "Ident = :userId" +
                    "SubstituteUserId = :substituteUserId " +
                    "and SubstitutionIsActive = 1) " +
                    "THEN 1 " +
                    "ELSE 0 " +
                    "END as Result; ",
                    new { userId = absenceUserId, substituteUserId = currentUserId });
            });
        }
    }
}
