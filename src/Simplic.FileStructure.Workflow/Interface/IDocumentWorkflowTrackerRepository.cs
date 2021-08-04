using Simplic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Defines the method for storing the tracking for document based workflows
    /// </summary>
    public interface IDocumentWorkflowTrackerRepository : IRepositoryBase<Guid, DocumentWorkflowTracker>
    {

    }
}
