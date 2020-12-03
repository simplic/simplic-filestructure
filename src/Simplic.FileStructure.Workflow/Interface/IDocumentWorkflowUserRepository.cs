using Simplic.Data;
using Simplic.FileStructure.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Defines the methods for storing the users in a document workflow 
    /// </summary>
    public interface IDocumentWorkflowUserRepository : IRepositoryBase<Guid, DocumentWorkflowUser>
    {
        /// <summary>
        /// Gets a <see cref="DocumentWorkflowUser"/> based on a user id
        /// </summary>
        /// <param name="userId">The key for a user</param>
        /// <returns>A user based on the id</returns>
        DocumentWorkflowUser Get(int userId);
    }
}
