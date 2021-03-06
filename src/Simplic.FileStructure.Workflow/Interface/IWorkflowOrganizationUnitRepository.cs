﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Data;
using Simplic.FileStructure.Workflow;

namespace Simplic.FileStructure.Workflow
{
    /// <summary>
    /// Defines methods for storing organization units for document based workflows 
    /// </summary>
    public interface IWorkflowOrganizationUnitRepository : IRepositoryBase<Guid, WorkflowOrganizationUnit>
    {
    }
}
