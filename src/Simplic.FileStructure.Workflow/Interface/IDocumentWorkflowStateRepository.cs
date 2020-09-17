using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Data;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Represents the document workflow state repository interfaces
    /// </summary>
     public interface IDocumentWorkflowStateRepository : IRepositoryBase<Guid, DocumentWorkflowState>
    {
    }
}
