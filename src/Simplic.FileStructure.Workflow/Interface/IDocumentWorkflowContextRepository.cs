using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Data;

namespace Simplic.FileStructure.Workflow
{
    public interface IDocumentWorkflowContextRepository : IRepositoryBase<Guid, DocumentWorkflowContext>
    {
    }
}
