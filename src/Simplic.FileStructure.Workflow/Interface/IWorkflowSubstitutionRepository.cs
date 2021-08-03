using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    public interface IWorkflowSubstitutionRepository
    {
        /// <summary>
        /// Checks if the current user has a substitute for the absence user
        /// </summary>
        /// <param name="currentUserId">The user id that is the substitute</param>
        /// <param name="absenceUserId">The user id that is in absence</param>
        /// <returns></returns>
        bool SubstitutionExists(long currentUserId, long absenceUserId);
    }
}
