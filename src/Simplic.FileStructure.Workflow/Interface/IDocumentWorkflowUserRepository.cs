using Simplic.Data;
using Simplic.FileStructure.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    public interface IDocumentWorkflowUserRepository : IRepositoryBase<Guid, DocumentWorkflowUser>
    {
        DocumentWorkflowUser Get(int userId);
    }
}
